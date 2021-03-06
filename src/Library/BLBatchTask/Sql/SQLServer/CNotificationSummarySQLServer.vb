''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CNotificationSummary.vb
' Class         : CNotificationSummary
' Description   : Table    NotificationSummary
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CNotificationSummary.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CNotificationSummarySQLServer
    Inherits CNotificationSummaryStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CNotificationSummaryStruct = TryCast(Obj, CNotificationSummaryStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO NotificationSummary ")
        SQL.Append("    ( ")
        SQL.Append("    [TellerId], ")
        SQL.Append("    [IBDNotificationCount], ")
        SQL.Append("    [PassThruNotificationCount], ")
        SQL.Append("    [BroadcastNotificationCount]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @TellerId, ")
        SQL.Append("    @IBDNotificationCount, ")
        SQL.Append("    @PassThruNotificationCount, ")
        SQL.Append("    @BroadcastNotificationCount) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TellerId", StructObj.TellerId)
        Command.Parameters.AddWithValue("IBDNotificationCount", StructObj.IBDNotificationCount)
        Command.Parameters.AddWithValue("PassThruNotificationCount", StructObj.PassThruNotificationCount)
        Command.Parameters.AddWithValue("BroadcastNotificationCount", StructObj.BroadcastNotificationCount)

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

        Dim StructObj As CNotificationSummaryStruct = TryCast(Obj, CNotificationSummaryStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE NotificationSummary ")
        SQL.Append(" SET ")
        SQL.Append("     [TellerId] = @TellerId, ")
        SQL.Append("     [IBDNotificationCount] = @IBDNotificationCount, ")
        SQL.Append("     [PassThruNotificationCount] = @PassThruNotificationCount, ")
        SQL.Append("     [BroadcastNotificationCount] = @BroadcastNotificationCount ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TellerId", StructObj.TellerId)
        Command.Parameters.AddWithValue("IBDNotificationCount", StructObj.IBDNotificationCount)
        Command.Parameters.AddWithValue("PassThruNotificationCount", StructObj.PassThruNotificationCount)
        Command.Parameters.AddWithValue("BroadcastNotificationCount", StructObj.BroadcastNotificationCount)

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

        Dim StructObj As CNotificationSummaryStruct = TryCast(Obj, CNotificationSummaryStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM NotificationSummary ")
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

        Dim StructObj As CNotificationSummaryStruct = TryCast(Obj, CNotificationSummaryStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [TellerId], ")
        SQL.Append("    [IBDNotificationCount], ")
        SQL.Append("    [PassThruNotificationCount], ")
        SQL.Append("    [BroadcastNotificationCount] ")
        SQL.Append(" FROM ")
        SQL.Append("     NotificationSummary ")
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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.TellerId = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.IBDNotificationCount = Row(0).Item(1)
        If Not IsDBNull(Row(0).Item(2)) Then StructObj.PassThruNotificationCount = Row(0).Item(2)
        If Not IsDBNull(Row(0).Item(3)) Then StructObj.BroadcastNotificationCount = Row(0).Item(3)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [TellerId], ")
        SQL.Append("    [IBDNotificationCount], ")
        SQL.Append("    [PassThruNotificationCount], ")
        SQL.Append("    [BroadcastNotificationCount] ")
        SQL.Append(" FROM ")
        SQL.Append("     NotificationSummary ")

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
            Dim StructObj As New CNotificationSummaryStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.TellerId = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.IBDNotificationCount = Row.Item(1)
            If Not IsDBNull(Row.Item(2)) Then StructObj.PassThruNotificationCount = Row.Item(2)
            If Not IsDBNull(Row.Item(3)) Then StructObj.BroadcastNotificationCount = Row.Item(3)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     NotificationSummary ")

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
            Return "SQLServer_NotificationSummary"
        End Get
    End Property

End Class

