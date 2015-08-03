Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CTransparentConverter
    Implements IDataConverter
    Implements IComponent

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "TransparentConverter"
        End Get
    End Property

    Public Property ConverterInfo As Library.Data.CCommInfo.CConverterInfo Implements IDataConverter.ConverterInfo

    Public Function Deserialise(ByRef Buffer() As Byte) As Library.Data.CMessage Implements IDataConverter.Deserialise
        Dim Message As New Library.Data.CMessage
        Dim Sb As New Text.StringBuilder
        For Each Item As Byte In Buffer
            Sb.Append(Item.ToString("X2"))
        Next
        Message.SetValue("HexData", Message)

        Return Message
    End Function

    Public Function Serialise(Message As Library.Data.CMessage) As IO.MemoryStream Implements IDataConverter.Serialise
        Dim Ms As New IO.MemoryStream
        Dim HexString As String = ""
        Message.GetValueByKey("HexData", HexString)

        Using Sw As New IO.StreamWriter(Ms)
            For i As Integer = 0 To HexString.Length - 1 Step 2
                Dim B As Byte = 0
                Ms.WriteByte(GetValue(HexString(i), HexString(i + 1)))
            Next
        End Using

        Return Ms
    End Function

    Private Function GetValue(FirstChar As String, SecondChar As String) As Byte
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
