''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CComboboxList.vb
' Class         : CComboboxList
' Description   : Table    ComboboxList
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CComboboxList.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CComboboxListSQLServer
    Inherits CComboboxListStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO ComboboxList ")
        SQL.Append("    ( ")
        SQL.Append("    [ComboId], ")
        SQL.Append("    [ComboValue], ")
        SQL.Append("    [ComboDescription], ")
        SQL.Append("    [Priority]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @ComboId, ")
        SQL.Append("    @ComboValue, ")
        SQL.Append("    @ComboDescription, ")
        SQL.Append("    @Priority) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ComboId", StructObj.ComboId)
        Command.Parameters.AddWithValue("ComboValue", StructObj.ComboValue)
        Command.Parameters.AddWithValue("ComboDescription", StructObj.ComboDescription)
        Command.Parameters.AddWithValue("Priority", StructObj.Priority)

        Try
            Dim Counter As Integer = Command.ExecuteNonQuery()
            If Counter <> 1 Then
                Throw New CBusinessException(CErrorCode.DATABASE_INSERT_FAIL, "0 Record Inserted")
            End If
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_INSERT_FAIL, ex)
        End Try
    End Sub

    Public Sub Update(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Update

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE ComboboxList ")
        SQL.Append(" SET ")
        SQL.Append("     [ComboId] = @ComboId, ")
        SQL.Append("     [ComboValue] = @ComboValue, ")
        SQL.Append("     [ComboDescription] = @ComboDescription, ")
        SQL.Append("     [Priority] = @Priority ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ComboId", StructObj.ComboId)
        Command.Parameters.AddWithValue("ComboValue", StructObj.ComboValue)
        Command.Parameters.AddWithValue("ComboDescription", StructObj.ComboDescription)
        Command.Parameters.AddWithValue("Priority", StructObj.Priority)

        Try
            Dim Counter As Integer = Command.ExecuteNonQuery()
            If Counter <> 1 Then
                Throw New CBusinessException(CErrorCode.DATABASE_UPDATE_FAIL, "0 Record Updated")
            End If
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_UPDATE_FAIL, ex)
        End Try
    End Sub

    Public Sub Remove(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Remove

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM ComboboxList ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString

        Try
            Dim Counter As Integer = Command.ExecuteNonQuery()
            If Counter <> 1 Then
                Throw New CBusinessException(CErrorCode.DATABASE_UPDATE_FAIL, "0 Record Deleted")
            End If
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_DELETE_FAIL, ex)
        End Try
    End Sub

    Public Sub Search(ByVal Adapter As IDatabaseAdapter, _
                           ByRef Obj As Object, ByVal Lock As Boolean?) Implements IDatabaseAccess.Search

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [ComboId], ")
        SQL.Append("    [ComboValue], ")
        SQL.Append("    [ComboDescription], ")
        SQL.Append("    [Priority] ")
        SQL.Append(" FROM ")
        SQL.Append("     ComboboxList ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString

        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter
        SQLAdapter.SelectCommand = Command

        Dim Data As New DataSet
        Try
            SQLAdapter.Fill(Data)
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)
        End Try

        Dim Row As Data.DataRowCollection = Data.Tables(0).Rows
        If Row.Count = 0 Then
            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, "Record Not Found")
        End If

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.ComboId = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.ComboValue = Row(0).Item(1)
        If Not IsDBNull(Row(0).Item(2)) Then StructObj.ComboDescription = Row(0).Item(2)
        If Not IsDBNull(Row(0).Item(3)) Then StructObj.Priority = Row(0).Item(3)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [ComboId], ")
        SQL.Append("    [ComboValue], ")
        SQL.Append("    [ComboDescription], ")
        SQL.Append("    [Priority] ")
        SQL.Append(" FROM ")
        SQL.Append("     ComboboxList ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString

        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter
        SQLAdapter.SelectCommand = Command

        Dim Data As New DataSet
        Try
            SQLAdapter.Fill(Data)
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)
        End Try

        Dim Rows As Data.DataRowCollection = Data.Tables(0).Rows
        If Rows.Count = 0 Then
            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, "Record Not Found")
        End If

        For Each Row As DataRow In Rows
            Dim StructObj As New CComboboxListStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.ComboId = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.ComboValue = Row.Item(1)
            If Not IsDBNull(Row.Item(2)) Then StructObj.ComboDescription = Row.Item(2)
            If Not IsDBNull(Row.Item(3)) Then StructObj.Priority = Row.Item(3)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Sub SearchByComboId(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CComboboxListStruct, _
                                      ByRef ComboboxListList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)
        ComboboxListList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [ComboId], ")
        SQL.Append("    [ComboValue], ")
        SQL.Append("    [ComboDescription], ")
        SQL.Append("    [Priority]  ")
        SQL.Append(" FROM ")
        SQL.Append("     ComboboxList ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [ComboId] = ")
        SQL.Append("       @ComboId ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     Priority ASC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ComboId", StructObj.ComboId)

        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter
        SQLAdapter.SelectCommand = Command

        Dim Data As New DataSet
        Try
            SQLAdapter.Fill(Data)
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)
        End Try

        Dim Row As DataRowCollection = Data.Tables(0).Rows
        If Row.Count = 0 Then
            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, "Record Not Found")
        End If

        For Idx As Integer = 0 To Data.Tables(0).Rows.Count - 1
            Dim DBStruct As New CComboboxListStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.ComboId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.ComboValue = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.ComboDescription = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Priority = Row(Idx).Item(3)

            ComboboxListList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchByTELECODE(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CComboboxListStruct, _
                                      ByRef ComboboxListList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)
        ComboboxListList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [ComboId], ")
        SQL.Append("    [ComboValue], ")
        SQL.Append("    [ComboDescription], ")
        SQL.Append("    [Priority]  ")
        SQL.Append(" FROM ")
        SQL.Append("     ComboboxList ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [ComboId] = ")
        SQL.Append("       @ComboId ")
        SQL.Append("     AND ")
        SQL.Append("     [ComboValue] = ")
        SQL.Append("       @ComboValue ")
        SQL.Append("     ) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ComboId", StructObj.ComboId)
        Command.Parameters.AddWithValue("ComboValue", StructObj.ComboValue)

        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter
        SQLAdapter.SelectCommand = Command

        Dim Data As New DataSet
        Try
            SQLAdapter.Fill(Data)
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)
        End Try

        Dim Row As DataRowCollection = Data.Tables(0).Rows
        If Row.Count = 0 Then
            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, "Record Not Found")
        End If

        For Idx As Integer = 0 To Data.Tables(0).Rows.Count - 1
            Dim DBStruct As New CComboboxListStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.ComboId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.ComboValue = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.ComboDescription = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Priority = Row(Idx).Item(3)

            ComboboxListList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchLikeComboId(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CComboboxListStruct, _
                                      ByRef ComboboxListList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CComboboxListStruct = TryCast(Obj, CComboboxListStruct)
        ComboboxListList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [ComboId], ")
        SQL.Append("    [ComboValue], ")
        SQL.Append("    [ComboDescription], ")
        SQL.Append("    [Priority]  ")
        SQL.Append(" FROM ")
        SQL.Append("     ComboboxList ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [ComboId] LIKE ")
        SQL.Append("       @ComboId ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     Priority ASC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ComboId", StructObj.ComboId)

        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter
        SQLAdapter.SelectCommand = Command

        Dim Data As New DataSet
        Try
            SQLAdapter.Fill(Data)
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)
        End Try

        Dim Row As DataRowCollection = Data.Tables(0).Rows
        If Row.Count = 0 Then
            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, "Record Not Found")
        End If

        For Idx As Integer = 0 To Data.Tables(0).Rows.Count - 1
            Dim DBStruct As New CComboboxListStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.ComboId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.ComboValue = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.ComboDescription = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Priority = Row(Idx).Item(3)

            ComboboxListList.Add(DBStruct)
        Next

    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     ComboboxList ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        
        Try
            Dim Counter As Integer = Command.ExecuteScalar()
            Return Count
        Catch ex As Exception
            CLog.Err(ex.Message)
            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)
        End Try
        
    End Function

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "SQLServer_ComboboxList"
        End Get
    End Property

End Class
