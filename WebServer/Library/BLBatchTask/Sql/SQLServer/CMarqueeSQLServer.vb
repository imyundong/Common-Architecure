''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CMarquee.vb
' Class         : CMarquee
' Description   : Table    Marquee
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CMarquee.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CMarqueeSQLServer
    Inherits CMarqueeStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CMarqueeStruct = TryCast(Obj, CMarqueeStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO Marquee ")
        SQL.Append("    ( ")
        SQL.Append("    [SystemId], ")
        SQL.Append("    [Content]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @SystemId, ")
        SQL.Append("    @Content) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("SystemId", StructObj.SystemId)
        Command.Parameters.AddWithValue("Content", StructObj.Content)

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

        Dim StructObj As CMarqueeStruct = TryCast(Obj, CMarqueeStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE Marquee ")
        SQL.Append(" SET ")
        SQL.Append("     [Content] = @Content ")
        SQL.Append(" WHERE ")
        SQL.Append("     [SystemId] = @SystemId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("SystemId", StructObj.SystemId)
        Command.Parameters.AddWithValue("Content", StructObj.Content)

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

        Dim StructObj As CMarqueeStruct = TryCast(Obj, CMarqueeStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM Marquee ")
        SQL.Append(" WHERE ")
        SQL.Append("     [SystemId] = @SystemId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("SystemId", StructObj.SystemId)

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

        Dim StructObj As CMarqueeStruct = TryCast(Obj, CMarqueeStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [SystemId], ")
        SQL.Append("    [Content] ")
        SQL.Append(" FROM ")
        SQL.Append("     Marquee ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE")
        SQL.Append("     [SystemId] = @SystemId ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("SystemId", StructObj.SystemId)

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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.SystemId = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.Content = Row(0).Item(1)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [SystemId], ")
        SQL.Append("    [Content] ")
        SQL.Append(" FROM ")
        SQL.Append("     Marquee ")

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
            Dim StructObj As New CMarqueeStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.SystemId = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.Content = Row.Item(1)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     Marquee ")

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
            Return "SQLServer_Marquee"
        End Get
    End Property

End Class

