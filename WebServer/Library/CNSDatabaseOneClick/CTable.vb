Public Class CTable
    Public Property Attr As New List(Of String)
    Public Property Id As String
    Public Property Fields As New List(Of CField)

    Public Property Query As New List(Of CQuery)
    Public Property Count As New List(Of CCount)

    Public Function IsValidFiled(ByVal FieldName As String) As Boolean
        For Each Field As CField In Fields
            If Field.Id.ToUpper = FieldName.ToUpper Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Function GetField(ByVal FieldName As String) As CField
        For Each Field As CField In Fields
            If Field.Id.ToUpper = FieldName.ToUpper Then
                Return Field
            End If
        Next

        Return Nothing
    End Function

    Public Function GetFieldsWithoutPK() As List(Of CField)
        Dim FieldList As New List(Of CField)
        For Each Field As CField In Fields
            If Field.Key = False Then
                FieldList.Add(Field)
            End If
        Next

        Return FieldList
    End Function

    Public Function GetPrimaryKey() As List(Of CField)
        Dim FieldList As New List(Of CField)
        For Each Field As CField In Fields
            If Field.Key = True Then
                FieldList.Add(Field)
            End If
        Next

        Return FieldList
    End Function
End Class
