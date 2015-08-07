Namespace Data
    <Serializable>
    Public Class CMessage
        Inherits MarshalByRefObject
        Implements ICloneable
        Property SystemId As String
        Property MessageId As String
        Property OriginalId As String
        Property IsAdviced As Boolean = False
        Property TxnCode As String
        Property IPAddress As String
        Property UserId As String
        Property TxnDate As Date = Now()
        Property SystemDate As Date
        Property ErrCode As ServerPlatform.Library.Workflow.CError.CErrorCode
        Property Keys As New List(Of String)
        Property Values As New List(Of Object)
        Property TableKeys As New List(Of String)
        Property TableValues As New List(Of System.Data.DataTable)

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Short)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Short.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Boolean)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Boolean.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Byte)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Byte.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Char)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Char.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Integer)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Integer.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Long)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Long.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Double)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Double.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Decimal)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Decimal.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Date)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Date.TryParse(Values(Idx), Value)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As String)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Value = Values(Idx)
            End If
        End Sub

        Sub GetValueByKey(ByVal Key As String, ByRef Value As Object)
            If Keys.Contains(Key) Then
                Dim Idx As Integer = Keys.IndexOf(Key)
                Try
                    Value = Values(Idx)
                Catch ex As Exception

                End Try
            End If
        End Sub

        Private Function ConvertToDataTable(ByVal Value As IEnumerable(Of Object))
            Dim Table As New System.Data.DataTable

            If Value.Count = 0 Then
                Return Table
            End If

            Dim DataType As Type = Value(0).GetType
            For Each PropertyInfo In DataType.GetProperties
                If PropertyInfo.CanWrite AndAlso PropertyInfo.CanRead Then
                    Table.Columns.Add(PropertyInfo.Name)
                    Table.Columns(Table.Columns.Count - 1).DataType = PropertyInfo.PropertyType
                End If
            Next

            For Each Item As Object In Value
                Dim Row = Table.NewRow
                For Idx As Integer = 0 To Table.Columns.Count - 1
                    Row.Item(Idx) = CallByName(Item, Table.Columns(Idx).ColumnName, CallType.Get)
                Next
                Table.Rows.Add(Row)
            Next

            Return Table
        End Function

        Sub SetValue(ByVal Key As String, ByVal Value As Integer())
            If Keys.Contains(Key) Then
                Values(Keys.IndexOf(Key)) = Value
            Else
                Keys.Add(Key)
                Values.Add(Value)
            End If
        End Sub

        Sub SetValue(ByVal Key As String, ByVal Value As Long)
            If Keys.Contains(Key) Then
                Values(Keys.IndexOf(Key)) = Value
            Else
                Keys.Add(Key)
                Values.Add(Value)
            End If
        End Sub
        Sub SetValue(ByVal Key As String, ByVal Value As IEnumerable(Of Object))
            Dim Table As DataTable = ConvertToDataTable(Value)
            Table.TableName = Key
            Dim Index As Integer = -1
            If TableKeys.Contains(Key) Then
                TableValues(Keys.IndexOf(Key)) = Table
            Else
                TableKeys.Add(Key)
                TableValues.Add(Table)
            End If

        End Sub

        Sub SetValue(ByVal Key As String, ByVal Value As String)
            If Keys.Contains(Key) Then
                Values(Keys.IndexOf(Key)) = Value
            Else
                Keys.Add(Key)
                Values.Add(Value)
            End If
        End Sub

        Sub SetValue(ByVal Key As String, ByVal Value As Integer)
            If Keys.Contains(Key) Then
                Values(Keys.IndexOf(Key)) = Value
            Else
                Keys.Add(Key)
                Values.Add(Value)
            End If
        End Sub


        Sub SetValue(ByVal Key As String, ByVal Value As Boolean)
            If Keys.Contains(Key) Then
                Values(Keys.IndexOf(Key)) = Value
            Else
                Keys.Add(Key)
                Values.Add(Value)
            End If
        End Sub

        Sub SetValue(ByVal Key As String, ByVal Value As Object)
            If Keys.Contains(Key) Then
                Values(Keys.IndexOf(Key)) = Value
            Else
                Keys.Add(Key)
                Values.Add(Value)
            End If
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim Message As New CMessage
            Message.ErrCode = ErrCode
            Message.MessageId = MessageId
            Message.OriginalId = OriginalId
            Message.SystemDate = SystemDate
            Message.TxnCode = TxnCode
            Message.TxnDate = TxnDate
            Message.UserId = UserId

            For Each Item As String In Keys
                Message.Keys.Add(Item)
            Next

            For Each Item As Object In Values
                Message.Values.Add(Item)
            Next

            Return Message
        End Function
    End Class
End Namespace