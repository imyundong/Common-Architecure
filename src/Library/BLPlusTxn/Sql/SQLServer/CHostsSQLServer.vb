''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CHosts.vb
' Class         : CHosts
' Description   : Table    Hosts
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CHosts.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CHostsSQLServer
    Inherits CHostsStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CHostsStruct = TryCast(Obj, CHostsStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO Hosts ")
        SQL.Append("    ( ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @HostId, ")
        SQL.Append("    @HostName) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("HostId", StructObj.HostId)
        Command.Parameters.AddWithValue("HostName", StructObj.HostName)

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

        Dim StructObj As CHostsStruct = TryCast(Obj, CHostsStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE Hosts ")
        SQL.Append(" SET ")
        SQL.Append("     [HostName] = @HostName ")
        SQL.Append(" WHERE ")
        SQL.Append("     [HostId] = @HostId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("HostId", StructObj.HostId)
        Command.Parameters.AddWithValue("HostName", StructObj.HostName)

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

        Dim StructObj As CHostsStruct = TryCast(Obj, CHostsStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM Hosts ")
        SQL.Append(" WHERE ")
        SQL.Append("     [HostId] = @HostId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("HostId", StructObj.HostId)

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

        Dim StructObj As CHostsStruct = TryCast(Obj, CHostsStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName] ")
        SQL.Append(" FROM ")
        SQL.Append("     Hosts ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE")
        SQL.Append("     [HostId] = @HostId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("HostId", StructObj.HostId)

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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.HostId = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.HostName = Row(0).Item(1)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName] ")
        SQL.Append(" FROM ")
        SQL.Append("     Hosts ")

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
            Dim StructObj As New CHostsStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.HostId = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.HostName = Row.Item(1)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     Hosts ")

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
            Return "SQLServer_Hosts"
        End Get
    End Property

End Class

