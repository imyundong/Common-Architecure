''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTxnPath.vb
' Class         : CTxnPath
' Description   : Table    TxnPath
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTxnPath.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CTxnPathSQLServer
    Inherits CTxnPathStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CTxnPathStruct = TryCast(Obj, CTxnPathStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO TxnPath ")
        SQL.Append("    ( ")
        SQL.Append("    [TxnPathId], ")
        SQL.Append("    [ParentId], ")
        SQL.Append("    [PathName], ")
        SQL.Append("    [Priority]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @TxnPathId, ")
        SQL.Append("    @ParentId, ")
        SQL.Append("    @PathName, ")
        SQL.Append("    @Priority) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TxnPathId", StructObj.TxnPathId)
        Command.Parameters.AddWithValue("ParentId", StructObj.ParentId)
        Command.Parameters.AddWithValue("PathName", StructObj.PathName)
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

        Dim StructObj As CTxnPathStruct = TryCast(Obj, CTxnPathStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE TxnPath ")
        SQL.Append(" SET ")
        SQL.Append("     [ParentId] = @ParentId, ")
        SQL.Append("     [PathName] = @PathName, ")
        SQL.Append("     [Priority] = @Priority ")
        SQL.Append(" WHERE ")
        SQL.Append("     [TxnPathId] = @TxnPathId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TxnPathId", StructObj.TxnPathId)
        Command.Parameters.AddWithValue("ParentId", StructObj.ParentId)
        Command.Parameters.AddWithValue("PathName", StructObj.PathName)
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

        Dim StructObj As CTxnPathStruct = TryCast(Obj, CTxnPathStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM TxnPath ")
        SQL.Append(" WHERE ")
        SQL.Append("     [TxnPathId] = @TxnPathId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TxnPathId", StructObj.TxnPathId)

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

        Dim StructObj As CTxnPathStruct = TryCast(Obj, CTxnPathStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [TxnPathId], ")
        SQL.Append("    [ParentId], ")
        SQL.Append("    [PathName], ")
        SQL.Append("    [Priority] ")
        SQL.Append(" FROM ")
        SQL.Append("     TxnPath ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE")
        SQL.Append("     [TxnPathId] = @TxnPathId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TxnPathId", StructObj.TxnPathId)

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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.TxnPathId = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.ParentId = Row(0).Item(1)
        If Not IsDBNull(Row(0).Item(2)) Then StructObj.PathName = Row(0).Item(2)
        If Not IsDBNull(Row(0).Item(3)) Then StructObj.Priority = Row(0).Item(3)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [TxnPathId], ")
        SQL.Append("    [ParentId], ")
        SQL.Append("    [PathName], ")
        SQL.Append("    [Priority] ")
        SQL.Append(" FROM ")
        SQL.Append("     TxnPath ")

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
            Dim StructObj As New CTxnPathStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.TxnPathId = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.ParentId = Row.Item(1)
            If Not IsDBNull(Row.Item(2)) Then StructObj.PathName = Row.Item(2)
            If Not IsDBNull(Row.Item(3)) Then StructObj.Priority = Row.Item(3)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Sub SearchByOrder(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CTxnPathStruct, _
                                      ByRef TxnPathList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CTxnPathStruct = TryCast(Obj, CTxnPathStruct)
        TxnPathList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [TxnPathId], ")
        SQL.Append("    [ParentId], ")
        SQL.Append("    [PathName], ")
        SQL.Append("    [Priority]  ")
        SQL.Append(" FROM ")
        SQL.Append("     TxnPath ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" ORDER BY ")
        SQL.Append("     ParentId ASC ")
        SQL.Append("     , ")
        SQL.Append("     Priority DESC ")

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

        Dim Row As DataRowCollection = Data.Tables(0).Rows
        If Row.Count = 0 Then
            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, "Record Not Found")
        End If

        For Idx As Integer = 0 To Data.Tables(0).Rows.Count - 1
            Dim DBStruct As New CTxnPathStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.TxnPathId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.ParentId = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.PathName = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Priority = Row(Idx).Item(3)

            TxnPathList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchByParent(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CTxnPathStruct, _
                                      ByRef TxnPathList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CTxnPathStruct = TryCast(Obj, CTxnPathStruct)
        TxnPathList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [TxnPathId], ")
        SQL.Append("    [ParentId], ")
        SQL.Append("    [PathName], ")
        SQL.Append("    [Priority]  ")
        SQL.Append(" FROM ")
        SQL.Append("     TxnPath ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [ParentId] = ")
        SQL.Append("       @ParentId ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     Priority DESC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ParentId", StructObj.ParentId)

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
            Dim DBStruct As New CTxnPathStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.TxnPathId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.ParentId = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.PathName = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Priority = Row(Idx).Item(3)

            TxnPathList.Add(DBStruct)
        Next

    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     TxnPath ")

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
            Return "SQLServer_TxnPath"
        End Get
    End Property

End Class
