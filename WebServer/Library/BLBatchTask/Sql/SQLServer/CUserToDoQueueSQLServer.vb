''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CUserToDoQueue.vb
' Class         : CUserToDoQueue
' Description   : Table    UserToDoQueue
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CUserToDoQueue.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CUserToDoQueueSQLServer
    Inherits CUserToDoQueueStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CUserToDoQueueStruct = TryCast(Obj, CUserToDoQueueStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO UserToDoQueue ")
        SQL.Append("    ( ")
        SQL.Append("    [QueueID], ")
        SQL.Append("    [DeviceID], ")
        SQL.Append("    [GroupID], ")
        SQL.Append("    [ID], ")
        SQL.Append("    [IDType]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @QueueID, ")
        SQL.Append("    @DeviceID, ")
        SQL.Append("    @GroupID, ")
        SQL.Append("    @ID, ")
        SQL.Append("    @IDType) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("QueueID", StructObj.QueueID)
        Command.Parameters.AddWithValue("DeviceID", StructObj.DeviceID)
        Command.Parameters.AddWithValue("GroupID", StructObj.GroupID)
        Command.Parameters.AddWithValue("ID", StructObj.ID)
        Command.Parameters.AddWithValue("IDType", StructObj.IDType)

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

        Dim StructObj As CUserToDoQueueStruct = TryCast(Obj, CUserToDoQueueStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE UserToDoQueue ")
        SQL.Append(" SET ")
        SQL.Append("     [QueueID] = @QueueID, ")
        SQL.Append("     [DeviceID] = @DeviceID, ")
        SQL.Append("     [GroupID] = @GroupID, ")
        SQL.Append("     [ID] = @ID, ")
        SQL.Append("     [IDType] = @IDType ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("QueueID", StructObj.QueueID)
        Command.Parameters.AddWithValue("DeviceID", StructObj.DeviceID)
        Command.Parameters.AddWithValue("GroupID", StructObj.GroupID)
        Command.Parameters.AddWithValue("ID", StructObj.ID)
        Command.Parameters.AddWithValue("IDType", StructObj.IDType)

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

        Dim StructObj As CUserToDoQueueStruct = TryCast(Obj, CUserToDoQueueStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM UserToDoQueue ")
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

        Dim StructObj As CUserToDoQueueStruct = TryCast(Obj, CUserToDoQueueStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [QueueID], ")
        SQL.Append("    [DeviceID], ")
        SQL.Append("    [GroupID], ")
        SQL.Append("    [ID], ")
        SQL.Append("    [IDType] ")
        SQL.Append(" FROM ")
        SQL.Append("     UserToDoQueue ")
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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.QueueID = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.DeviceID = Row(0).Item(1)
        If Not IsDBNull(Row(0).Item(2)) Then StructObj.GroupID = Row(0).Item(2)
        If Not IsDBNull(Row(0).Item(3)) Then StructObj.ID = Row(0).Item(3)
        If Not IsDBNull(Row(0).Item(4)) Then StructObj.IDType = Row(0).Item(4)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [QueueID], ")
        SQL.Append("    [DeviceID], ")
        SQL.Append("    [GroupID], ")
        SQL.Append("    [ID], ")
        SQL.Append("    [IDType] ")
        SQL.Append(" FROM ")
        SQL.Append("     UserToDoQueue ")

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
            Dim StructObj As New CUserToDoQueueStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.QueueID = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.DeviceID = Row.Item(1)
            If Not IsDBNull(Row.Item(2)) Then StructObj.GroupID = Row.Item(2)
            If Not IsDBNull(Row.Item(3)) Then StructObj.ID = Row.Item(3)
            If Not IsDBNull(Row.Item(4)) Then StructObj.IDType = Row.Item(4)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Sub SearchByID(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CUserToDoQueueStruct, _
                                      ByRef UserToDoQueueList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CUserToDoQueueStruct = TryCast(Obj, CUserToDoQueueStruct)
        UserToDoQueueList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [QueueID], ")
        SQL.Append("    [DeviceID], ")
        SQL.Append("    [GroupID], ")
        SQL.Append("    [ID], ")
        SQL.Append("    [IDType]  ")
        SQL.Append(" FROM ")
        SQL.Append("     UserToDoQueue ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [ID] = ")
        SQL.Append("       @ID ")
        SQL.Append("     ) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ID", StructObj.ID)

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
            Dim DBStruct As New CUserToDoQueueStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.QueueID = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.DeviceID = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.GroupID = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.ID = Row(Idx).Item(3)
            If Not IsDBNull(Row(Idx).Item(4)) Then DBStruct.IDType = Row(Idx).Item(4)

            UserToDoQueueList.Add(DBStruct)
        Next

    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     UserToDoQueue ")

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
            Return "SQLServer_UserToDoQueue"
        End Get
    End Property

End Class
