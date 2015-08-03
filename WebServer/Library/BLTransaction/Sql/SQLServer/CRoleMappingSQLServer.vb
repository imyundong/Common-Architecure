''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CRoleMapping.vb
' Class         : CRoleMapping
' Description   : Table    RoleMapping
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CRoleMapping.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CRoleMappingSQLServer
    Inherits CRoleMappingStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CRoleMappingStruct = TryCast(Obj, CRoleMappingStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO RoleMapping ")
        SQL.Append("    ( ")
        SQL.Append("    [ETabRole], ")
        SQL.Append("    [TellerRoleId]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @ETabRole, ")
        SQL.Append("    @TellerRoleId) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ETabRole", StructObj.ETabRole)
        Command.Parameters.AddWithValue("TellerRoleId", StructObj.TellerRoleId)

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

        Dim StructObj As CRoleMappingStruct = TryCast(Obj, CRoleMappingStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE RoleMapping ")
        SQL.Append(" SET ")
        SQL.Append("     [ETabRole] = @ETabRole, ")
        SQL.Append("     [TellerRoleId] = @TellerRoleId ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ETabRole", StructObj.ETabRole)
        Command.Parameters.AddWithValue("TellerRoleId", StructObj.TellerRoleId)

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

        Dim StructObj As CRoleMappingStruct = TryCast(Obj, CRoleMappingStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM RoleMapping ")
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

        Dim StructObj As CRoleMappingStruct = TryCast(Obj, CRoleMappingStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [ETabRole], ")
        SQL.Append("    [TellerRoleId] ")
        SQL.Append(" FROM ")
        SQL.Append("     RoleMapping ")
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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.ETabRole = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.TellerRoleId = Row(0).Item(1)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [ETabRole], ")
        SQL.Append("    [TellerRoleId] ")
        SQL.Append(" FROM ")
        SQL.Append("     RoleMapping ")

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
            Dim StructObj As New CRoleMappingStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.ETabRole = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.TellerRoleId = Row.Item(1)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Sub SearchGetRoleByETabRole(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CRoleMappingStruct, _
                                      ByRef RoleMappingList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CRoleMappingStruct = TryCast(Obj, CRoleMappingStruct)
        RoleMappingList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [ETabRole], ")
        SQL.Append("    [TellerRoleId]  ")
        SQL.Append(" FROM ")
        SQL.Append("     RoleMapping ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [ETabRole] = ")
        SQL.Append("       @ETabRole ")
        SQL.Append("     ) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("ETabRole", StructObj.ETabRole)

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
            Dim DBStruct As New CRoleMappingStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.ETabRole = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.TellerRoleId = Row(Idx).Item(1)

            RoleMappingList.Add(DBStruct)
        Next

    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     RoleMapping ")

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
            Return "SQLServer_RoleMapping"
        End Get
    End Property

End Class
