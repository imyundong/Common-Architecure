''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COperdate.vb
' Class         : COperdate
' Description   : Table    Operdate
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COperdate.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class COperdateSQLServer
    Inherits COperdateStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO Operdate ")
        SQL.Append("    ( ")
        SQL.Append("    [OP_DATE], ")
        SQL.Append("    [DATE_FLAG], ")
        SQL.Append("    [IS_OP_DATE]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @OP_DATE, ")
        SQL.Append("    @DATE_FLAG, ")
        SQL.Append("    @IS_OP_DATE) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("OP_DATE", StructObj.OP_DATE)
        Command.Parameters.AddWithValue("DATE_FLAG", StructObj.DATE_FLAG)
        Command.Parameters.AddWithValue("IS_OP_DATE", StructObj.IS_OP_DATE)

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

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE Operdate ")
        SQL.Append(" SET ")
        SQL.Append("     [OP_DATE] = @OP_DATE, ")
        SQL.Append("     [DATE_FLAG] = @DATE_FLAG, ")
        SQL.Append("     [IS_OP_DATE] = @IS_OP_DATE ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("OP_DATE", StructObj.OP_DATE)
        Command.Parameters.AddWithValue("DATE_FLAG", StructObj.DATE_FLAG)
        Command.Parameters.AddWithValue("IS_OP_DATE", StructObj.IS_OP_DATE)

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

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM Operdate ")
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

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [OP_DATE], ")
        SQL.Append("    [DATE_FLAG], ")
        SQL.Append("    [IS_OP_DATE] ")
        SQL.Append(" FROM ")
        SQL.Append("     Operdate ")
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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.OP_DATE = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.DATE_FLAG = Row(0).Item(1)
        If Not IsDBNull(Row(0).Item(2)) Then StructObj.IS_OP_DATE = Row(0).Item(2)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [OP_DATE], ")
        SQL.Append("    [DATE_FLAG], ")
        SQL.Append("    [IS_OP_DATE] ")
        SQL.Append(" FROM ")
        SQL.Append("     Operdate ")

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
            Dim StructObj As New COperdateStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.OP_DATE = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.DATE_FLAG = Row.Item(1)
            If Not IsDBNull(Row.Item(2)) Then StructObj.IS_OP_DATE = Row.Item(2)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Sub SearchByOperDate(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As COperdateStruct, _
                                      ByRef OperdateList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)
        OperdateList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [OP_DATE], ")
        SQL.Append("    [DATE_FLAG], ")
        SQL.Append("    [IS_OP_DATE]  ")
        SQL.Append(" FROM ")
        SQL.Append("     Operdate ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [OP_DATE] = ")
        SQL.Append("       @OP_DATE ")
        SQL.Append("     AND ")
        SQL.Append("     [IS_OP_DATE] = ")
        SQL.Append("       @IS_OP_DATE ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     OP_DATE ASC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("OP_DATE", StructObj.OP_DATE)
        Command.Parameters.AddWithValue("IS_OP_DATE", StructObj.IS_OP_DATE)

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
            Dim DBStruct As New COperdateStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.OP_DATE = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.DATE_FLAG = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.IS_OP_DATE = Row(Idx).Item(2)

            OperdateList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchNextDateByOperDate(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As COperdateStruct, _
                                      ByRef OperdateList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)
        OperdateList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [OP_DATE], ")
        SQL.Append("    [DATE_FLAG], ")
        SQL.Append("    [IS_OP_DATE]  ")
        SQL.Append(" FROM ")
        SQL.Append("     Operdate ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [OP_DATE] > ")
        SQL.Append("       @OP_DATE ")
        SQL.Append("     AND ")
        SQL.Append("     [IS_OP_DATE] = ")
        SQL.Append("       @IS_OP_DATE ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     OP_DATE ASC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("OP_DATE", StructObj.OP_DATE)
        Command.Parameters.AddWithValue("IS_OP_DATE", StructObj.IS_OP_DATE)

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
            Dim DBStruct As New COperdateStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.OP_DATE = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.DATE_FLAG = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.IS_OP_DATE = Row(Idx).Item(2)

            OperdateList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchPreDateByOperDate(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As COperdateStruct, _
                                      ByRef OperdateList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As COperdateStruct = TryCast(Obj, COperdateStruct)
        OperdateList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [OP_DATE], ")
        SQL.Append("    [DATE_FLAG], ")
        SQL.Append("    [IS_OP_DATE]  ")
        SQL.Append(" FROM ")
        SQL.Append("     Operdate ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [OP_DATE] < ")
        SQL.Append("       @OP_DATE ")
        SQL.Append("     AND ")
        SQL.Append("     [IS_OP_DATE] = ")
        SQL.Append("       @IS_OP_DATE ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     OP_DATE DESC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("OP_DATE", StructObj.OP_DATE)
        Command.Parameters.AddWithValue("IS_OP_DATE", StructObj.IS_OP_DATE)

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
            Dim DBStruct As New COperdateStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.OP_DATE = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.DATE_FLAG = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.IS_OP_DATE = Row(Idx).Item(2)

            OperdateList.Add(DBStruct)
        Next

    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     Operdate ")

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
            Return "SQLServer_Operdate"
        End Get
    End Property

End Class

