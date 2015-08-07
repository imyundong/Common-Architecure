Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CBancsDataConverter
    Implements IDataConverter
    Implements IComponent

    Public Function Deserialise(ByRef Buffer() As Byte) As Library.Data.CMessage Implements IDataConverter.Deserialise
        Dim Message As New Library.Data.CMessage
        Dim Encoding As Text.Encoding
        Try
            ' Get Encoding
            Encoding = Text.Encoding.GetEncoding(CInt(ConverterInfo.Encoding))
        Catch ex As Exception
            ' Get Encoding
            Encoding = Text.Encoding.GetEncoding(ConverterInfo.Encoding)
        End Try
 
        ' Load Master Transactions Layout
        Dim XmlDoc As New Xml.XmlDocument

        XmlDoc.Load(CUtility.ServerPath(ConverterInfo.WorkingPath + IO.Path.DirectorySeparatorChar + ConverterInfo.Master))
        Dim Ms As New IO.MemoryStream(Buffer)

        CLog.Info("Deserilise...")
        Deserialise(XmlDoc, Ms, Message, Encoding)
        If Message.Keys.Contains("MessageId") Then
            Message.GetValueByKey("MessageId", Message.MessageId)
        End If

        Return Message
    End Function

    Private Sub Deserialise(ByVal XmlDoc As Xml.XmlNode, ByVal Ms As IO.MemoryStream, ByVal Message As Library.Data.CMessage, ByVal Encoding As Text.Encoding)
        Dim TxSection As Xml.XmlNode = XmlDoc.SelectSingleNode("GROUP/RX")

        For Each Node As Xml.XmlNode In TxSection.ChildNodes
            If Node.Name.ToUpper = "FIELD" Then
                Dim Formatter As FieldFormatter = FieldFormatter.GetFieldFormatter(Node.Attributes("Format").Value, Encoding)
                Dim Length As Integer = Formatter.Length
                If Length = -1 Then
                    Length = Ms.Length - Ms.Position
                End If

                Dim Buf(Length - 1) As Byte
                Ms.Read(Buf, 0, Buf.Length)
                Dim Value As Object = Formatter.GetValue(Buf)
                Message.SetValue(Node.Attributes("ID").Value, Value)

                Dim MappingValue As Xml.XmlNode = Node.Attributes("Value1")
                If MappingValue IsNot Nothing AndAlso MappingValue.Value = Value.ToString Then Message.SetValue(Node.Attributes("ID").Value + "Value", True)
                MappingValue = Node.Attributes("Value2")
                If MappingValue IsNot Nothing AndAlso MappingValue.Value = Value.ToString Then Message.SetValue(Node.Attributes("ID").Value + "Value", True)

            ElseIf Node.Name.ToUpper = "SELECT" Then
                ' Process Switch
                Dim FieldName As String = Node.Attributes("switch").InnerText
                Dim Field As String = ""
                Message.GetValueByKey(FieldName, Field)

                For Each CaseNode As Xml.XmlNode In Node.ChildNodes
                    If CaseNode.Name.ToUpper = "CASE" Then
                        If Field = CaseNode.Attributes("Value").InnerText Then
                            Field = CaseNode.InnerText
                            Exit For
                        End If
                    ElseIf CaseNode.Name.ToUpper = "CASEELSE" Then
                        Field = CaseNode.InnerText
                        Exit For
                    End If
                Next

                Dim SubXmlDoc As New Xml.XmlDocument
                SubXmlDoc.Load(CUtility.ServerPath(ConverterInfo.WorkingPath + IO.Path.DirectorySeparatorChar + Field + ".xml"))
                Deserialise(SubXmlDoc, Ms, Message, Encoding)
            End If
        Next

        Message.GetValueByKey("TxnCode", Message.TxnCode)
    End Sub

    Private Sub Serialise(ByVal XmlDoc As Xml.XmlNode, ByVal Ms As IO.MemoryStream, ByVal Message As Library.Data.CMessage, ByVal Encoding As Text.Encoding)
        Dim TxSection As Xml.XmlNode = XmlDoc.SelectSingleNode("GROUP/TX")
        For Each Node As Xml.XmlNode In TxSection.ChildNodes
            If Node.Name.ToUpper = "FIELD" Then
                Dim Formatter As FieldFormatter = FieldFormatter.GetFieldFormatter(Node.Attributes("Format").Value, Encoding)
                Dim Value As Object = Nothing
                Message.GetValueByKey(Node.Attributes("ID").Value, Value)

                If Value Is Nothing OrElse String.IsNullOrEmpty(Value.ToString) Then
                    If Not String.IsNullOrEmpty(Node.InnerText) Then
                        Value = Node.InnerText
                    Else
                        Value = ""
                    End If
                End If

                Dim Buffer() As Byte = Formatter.GetBytes(Value)

                If Buffer.Length > Formatter.Length And Formatter.Length <> -1 Then
                    Throw New CError.CBusinessException(CError.CErrorCode.FIELD_FORMAT_ERROR, _
                                                        "File {0}({1}) is too Large ", Node.Attributes("ID").Value, Buffer)
                Else
                    Ms.Write(Buffer, 0, Buffer.Length)
                End If
            ElseIf Node.Name.ToUpper = "SELECT" Then
                ' Process Switch
                Dim FieldName As String = Node.Attributes("switch").InnerText
                Dim Field As String = ""
                Message.GetValueByKey(FieldName, Field)

                For Each CaseNode As Xml.XmlNode In Node.ChildNodes
                    If CaseNode.Name.ToUpper = "CASE" Then
                        If Field = CaseNode.Attributes("Value").InnerText Then
                            Field = CaseNode.InnerText
                            Exit For
                        End If
                    ElseIf CaseNode.Name.ToUpper = "CASEELSE" Then
                        Field = CaseNode.InnerText
                        Exit For
                    End If
                Next

                Dim SubXmlDoc As New Xml.XmlDocument
                SubXmlDoc.Load(CUtility.ServerPath(ConverterInfo.WorkingPath + IO.Path.DirectorySeparatorChar + Field + ".xml"))
                Serialise(SubXmlDoc, Ms, Message, Encoding)
            End If
        Next
    End Sub

    Public Function Serialise(Message As Library.Data.CMessage) As IO.MemoryStream Implements IDataConverter.Serialise
        ' Prepare All Data
        Message.SetValue("UserId", Message.UserId)
        Message.SetValue("TxnDate", Message.TxnDate)
        Message.SetValue("SystemDate", Message.SystemDate)
        Message.SetValue("TxnCode", Message.TxnCode)
        Message.SetValue("ErrCode", CInt(Message.ErrCode))

        Dim Encoding As Text.Encoding
        Try
            ' Get Encoding
            Encoding = Text.Encoding.GetEncoding(CInt(ConverterInfo.Encoding))
        Catch ex As Exception
            ' Get Encoding
            Encoding = Text.Encoding.GetEncoding(ConverterInfo.Encoding)
        End Try

        Dim MemoryStream As New IO.MemoryStream
        ' Load Master Transactions Layout
        Dim MasterDocument As New Xml.XmlDocument
        MasterDocument.Load(CUtility.ServerPath(ConverterInfo.WorkingPath + IO.Path.DirectorySeparatorChar + ConverterInfo.Master))
        Serialise(MasterDocument, MemoryStream, Message, Encoding)

        Return MemoryStream
    End Function

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "BancsDataConverter"
        End Get
    End Property

    Private MustInherit Class FieldFormatter
        Property Encoding As Text.Encoding
        Property FormatterIndicator As String
        Property Length As Integer

        MustOverride Function GetValue(ByVal Value() As Byte) As Object
        Sub New(FormatterIndicator As String, Encoding As Text.Encoding)
            Me.Encoding = Encoding
            Me.FormatterIndicator = FormatterIndicator
        End Sub
        MustOverride Function GetBytes(ByVal Value As String) As Byte()
        Shared Function GetFieldFormatter(FormatterIndicator As String, Encoding As Text.Encoding) As FieldFormatter
            If String.IsNullOrEmpty(FormatterIndicator) Then Throw New Exception("Invalid Formatter Indicatior")
            If FormatterIndicator.StartsWith("X") Then
                Return New StringFormatter(FormatterIndicator, Encoding)
            ElseIf FormatterIndicator.StartsWith("9") Then
                Return New NumericFormatter(FormatterIndicator, Encoding)
            ElseIf FormatterIndicator.StartsWith("B") Then
                Return New BinaryFormatter(FormatterIndicator, Encoding)
            ElseIf FormatterIndicator.Contains("y") OrElse _
                FormatterIndicator.Contains("m") OrElse _
                FormatterIndicator.Contains("Y") OrElse _
                FormatterIndicator.Contains("M") Then
                Return New DateFormatter(FormatterIndicator, Encoding)
            Else
                Throw New Exception("Invalid Formatter Indicatior")
            End If
        End Function

    End Class

    Private Class NumericFormatter
        Inherits FieldFormatter

        Property Dec As Integer = 0
        Private Sign As CSign = CSign.NONE

        Private Enum CSign As Integer
            NONE = 0
            LEFT = 1
            RIGHT = 2
        End Enum
        Sub New(FormatterIndicator As String, Encoding As Text.Encoding)
            MyBase.New(FormatterIndicator, Encoding)
            If FormatterIndicator.StartsWith("S") Then
                Sign = CSign.LEFT
            End If

            If FormatterIndicator.EndsWith("S") Then
                Sign = CSign.RIGHT
            End If
            FormatterIndicator = FormatterIndicator.Replace("S", "")

            If FormatterIndicator.Contains("(") Then
                Dim Temp() As String = FormatterIndicator.Split("(")
                Length = Temp(1).Substring(0, Temp(1).IndexOf(")"))

                If FormatterIndicator.Contains("V") Then
                    Temp = FormatterIndicator.Split("V")

                    If Temp(1).Contains("(") Then
                        Temp = Temp(1).Split("(")
                        Dec = Temp(1).Substring(0, Temp(1).IndexOf(")"))
                        Length += Dec
                    Else
                        Length += Temp(1).Length
                        Dec = Temp(1).Length
                    End If
                End If
            Else
                Length = FormatterIndicator.Trim.Length
            End If

            If Sign <> CSign.NONE Then Length += 1

        End Sub
        Public Overrides Function GetBytes(Value As String) As Byte()
            Dim Val As Decimal = 0
            Decimal.TryParse(Value, Val)
            Val = Val * 10 ^ (Dec)

            Dim Buf(Length - 1) As Byte

            Dim StringValue As String = CLng(Val).ToString
            If Sign = CSign.LEFT And Val < 0 Then StringValue = StringValue.Replace("-", "")
            If Sign = CSign.RIGHT Then
                If Val > 0 Then
                    StringValue = StringValue + "+"
                Else
                    StringValue = StringValue.Replace("-", "")
                    StringValue = StringValue + "1"
                End If
            End If

            Dim TempBuf() As Byte = Encoding.GetBytes(StringValue)
            If TempBuf.Length > Length Then
                Return TempBuf
            Else
                TempBuf.CopyTo(Buf, Length - TempBuf.Length)
            End If

            For i As Integer = 0 To Length - TempBuf.Length - 1
                Buf(i) = Encoding.GetBytes("0")(0)
            Next

            If Sign = CSign.LEFT And Val < 0 Then StringValue = Buf(0) = Encoding.GetBytes("-")(0)

            Return Buf
        End Function

        Public Overrides Function GetValue(Value() As Byte) As Object
            Dim StringValue As String = Encoding.GetString(Value)
            Dim Sign As Integer = 1
            If StringValue.Contains("+") Then
                StringValue = StringValue.Replace("+", "")
            ElseIf StringValue.Contains("-") Then
                Sign = -1
                StringValue = StringValue.Replace("-", "")
            End If
            Dim Val As Decimal
            Decimal.TryParse(StringValue, Val)

            Return Val / 10 ^ (Dec) * Sign
        End Function
    End Class

    Private Class BinaryFormatter
        Inherits FieldFormatter
        Sub New(FormatterIndicator As String, Encoding As Text.Encoding)
            MyBase.New(FormatterIndicator, Encoding)

            If FormatterIndicator.Contains("(") Then
                Dim Temp() As String = FormatterIndicator.Split("(")
                Dim Desc As String = Temp(1).Substring(0, Temp(1).IndexOf(")"))
                If Desc.ToUpper() = "MAX" Then
                    Length = -1
                Else
                    Length = CInt(Desc)
                End If
            Else
                Length = FormatterIndicator.Trim.Length
            End If
        End Sub

        Public Overrides Function GetBytes(Value As String) As Byte()
            Dim Ms As New IO.MemoryStream
            Using Sw As New IO.StreamWriter(Ms)
                For i As Integer = 0 To Value.Length - 1 Step 2
                    Dim B As Byte = 0
                    Ms.WriteByte(GetValue(Value(i), Value(i + 1)))
                Next
            End Using

            Return Ms.ToArray
        End Function

        Public Overrides Function GetValue(Value() As Byte) As Object
            Dim Sb As New Text.StringBuilder
            For Each Item As Byte In Value
                Sb.Append(Item.ToString("X2"))
            Next

            Return Sb.ToString
        End Function

        Private Overloads Function GetValue(FirstChar As String, SecondChar As String) As Byte
            Dim B As Byte = Asc(FirstChar)
            If B >= 65 Then
                B = (B - 55) * 16
            Else
                B = (B - 48) * 16
            End If

            Dim C As Byte = Asc(SecondChar)
            If C >= 65 Then
                C = (C - 55)
            Else
                C = (C - 48)
            End If

            Return B + C
        End Function
    End Class

    Private Class StringFormatter
        Inherits FieldFormatter

        Sub New(FormatterIndicator As String, Encoding As Text.Encoding)
            MyBase.New(FormatterIndicator, Encoding)

            If FormatterIndicator.Contains("(") Then
                Dim Temp() As String = FormatterIndicator.Split("(")
                Dim Desc As String = Temp(1).Substring(0, Temp(1).IndexOf(")"))
                If Desc.ToUpper() = "MAX" Then
                    Length = -1
                Else
                    Length = CInt(Desc)
                End If
            Else
                Length = FormatterIndicator.Trim.Length
            End If
        End Sub

        Public Overrides Function GetBytes(Value As String) As Byte()
            Dim Buf(Length - 1) As Byte
            Dim TempBuf() As Byte = Encoding.GetBytes(Value)
            If TempBuf.Length <= Length Then
                TempBuf.CopyTo(Buf, 0)
            Else
                Return TempBuf
            End If

            For i As Integer = TempBuf.Length To Length - 1
                Buf(i) = Encoding.GetBytes(" ")(0)
            Next

            Return Buf
        End Function

        Public Overrides Function GetValue(Value() As Byte) As Object
            Return Trim(Encoding.GetString(Value))
        End Function
    End Class

    Private Class DateFormatter
        Inherits FieldFormatter

        Sub New(FormatterIndicator As String, Encoding As Text.Encoding)
            MyBase.New(FormatterIndicator, Encoding)
            Throw New NotImplementedException("Data Formatter is not Implemeted")
        End Sub

        Public Overrides Function GetBytes(Value As String) As Byte()
            Throw New NotImplementedException("Data Formatter is not Implemeted")
        End Function

        Public Overrides Function GetValue(Value() As Byte) As Object
            Return Now
        End Function
    End Class

    Public Property ConverterInfo As Library.Data.CCommInfo.CConverterInfo Implements IDataConverter.ConverterInfo
End Class
