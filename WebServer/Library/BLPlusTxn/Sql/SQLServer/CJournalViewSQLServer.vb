''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CJournalView.vb
' Class         : CJournalView
' Description   : Table    JournalView
'               : Database SQLServer
'               : This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CJournalView.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow.CError

Public Class CJournalViewSQLServer
    Inherits CJournalViewStruct
    Implements IDatabaseAccess
    Implements IComponent

    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _
        Implements IDatabaseAccess.Insert

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" INSERT INTO JournalView ")
        SQL.Append("    ( ")
        SQL.Append("    [JournalId], ")
        SQL.Append("    [GroupId], ")
        SQL.Append("    [TxnCode], ")
        SQL.Append("    [Title], ")
        SQL.Append("    [Status], ")
        SQL.Append("    [StatusDescription], ")
        SQL.Append("    [ErrCode], ")
        SQL.Append("    [ErrDescription], ")
        SQL.Append("    [ProcTime], ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName], ")
        SQL.Append("    [TraceNo], ")
        SQL.Append("    [SystemDate], ")
        SQL.Append("    [BusinessDate], ")
        SQL.Append("    [Teller], ")
        SQL.Append("    [TellerName], ")
        SQL.Append("    [BranchId], ")
        SQL.Append("    [Supervisor], ")
        SQL.Append("    [SupervisorName], ")
        SQL.Append("    [Account], ")
        SQL.Append("    [Currency], ")
        SQL.Append("    [TxnAmount], ")
        SQL.Append("    [Terminal], ")
        SQL.Append("    [PageData], ")
        SQL.Append("    [OverrideId], ")
        SQL.Append("    [ReversalTxn]) ")
        SQL.Append(" VALUES ")
        SQL.Append("    ( ")
        SQL.Append("    @JournalId, ")
        SQL.Append("    @GroupId, ")
        SQL.Append("    @TxnCode, ")
        SQL.Append("    @Title, ")
        SQL.Append("    @Status, ")
        SQL.Append("    @StatusDescription, ")
        SQL.Append("    @ErrCode, ")
        SQL.Append("    @ErrDescription, ")
        SQL.Append("    @ProcTime, ")
        SQL.Append("    @HostId, ")
        SQL.Append("    @HostName, ")
        SQL.Append("    @TraceNo, ")
        SQL.Append("    @SystemDate, ")
        SQL.Append("    @BusinessDate, ")
        SQL.Append("    @Teller, ")
        SQL.Append("    @TellerName, ")
        SQL.Append("    @BranchId, ")
        SQL.Append("    @Supervisor, ")
        SQL.Append("    @SupervisorName, ")
        SQL.Append("    @Account, ")
        SQL.Append("    @Currency, ")
        SQL.Append("    @TxnAmount, ")
        SQL.Append("    @Terminal, ")
        SQL.Append("    @PageData, ")
        SQL.Append("    @OverrideId, ")
        SQL.Append("    @ReversalTxn) ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("JournalId", StructObj.JournalId)
        Command.Parameters.AddWithValue("GroupId", StructObj.GroupId)
        Command.Parameters.AddWithValue("TxnCode", StructObj.TxnCode)
        Command.Parameters.AddWithValue("Title", StructObj.Title)
        Command.Parameters.AddWithValue("Status", StructObj.Status)
        Command.Parameters.AddWithValue("StatusDescription", StructObj.StatusDescription)
        Command.Parameters.AddWithValue("ErrCode", StructObj.ErrCode)
        Command.Parameters.AddWithValue("ErrDescription", StructObj.ErrDescription)
        Command.Parameters.AddWithValue("ProcTime", StructObj.ProcTime)
        Command.Parameters.AddWithValue("HostId", StructObj.HostId)
        Command.Parameters.AddWithValue("HostName", StructObj.HostName)
        Command.Parameters.AddWithValue("TraceNo", StructObj.TraceNo)
        Command.Parameters.AddWithValue("SystemDate", StructObj.SystemDate)
        Command.Parameters.AddWithValue("BusinessDate", StructObj.BusinessDate)
        Command.Parameters.AddWithValue("Teller", StructObj.Teller)
        Command.Parameters.AddWithValue("TellerName", StructObj.TellerName)
        Command.Parameters.AddWithValue("BranchId", StructObj.BranchId)
        Command.Parameters.AddWithValue("Supervisor", StructObj.Supervisor)
        Command.Parameters.AddWithValue("SupervisorName", StructObj.SupervisorName)
        Command.Parameters.AddWithValue("Account", StructObj.Account)
        Command.Parameters.AddWithValue("Currency", StructObj.Currency)
        Command.Parameters.AddWithValue("TxnAmount", StructObj.TxnAmount)
        Command.Parameters.AddWithValue("Terminal", StructObj.Terminal)
        Command.Parameters.AddWithValue("PageData", StructObj.PageData)
        Command.Parameters.AddWithValue("OverrideId", StructObj.OverrideId)
        Command.Parameters.AddWithValue("ReversalTxn", StructObj.ReversalTxn)

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

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" UPDATE JournalView ")
        SQL.Append(" SET ")
        SQL.Append("     [JournalId] = @JournalId, ")
        SQL.Append("     [GroupId] = @GroupId, ")
        SQL.Append("     [TxnCode] = @TxnCode, ")
        SQL.Append("     [Title] = @Title, ")
        SQL.Append("     [Status] = @Status, ")
        SQL.Append("     [StatusDescription] = @StatusDescription, ")
        SQL.Append("     [ErrCode] = @ErrCode, ")
        SQL.Append("     [ErrDescription] = @ErrDescription, ")
        SQL.Append("     [ProcTime] = @ProcTime, ")
        SQL.Append("     [HostId] = @HostId, ")
        SQL.Append("     [HostName] = @HostName, ")
        SQL.Append("     [TraceNo] = @TraceNo, ")
        SQL.Append("     [SystemDate] = @SystemDate, ")
        SQL.Append("     [BusinessDate] = @BusinessDate, ")
        SQL.Append("     [Teller] = @Teller, ")
        SQL.Append("     [TellerName] = @TellerName, ")
        SQL.Append("     [BranchId] = @BranchId, ")
        SQL.Append("     [Supervisor] = @Supervisor, ")
        SQL.Append("     [SupervisorName] = @SupervisorName, ")
        SQL.Append("     [Account] = @Account, ")
        SQL.Append("     [Currency] = @Currency, ")
        SQL.Append("     [TxnAmount] = @TxnAmount, ")
        SQL.Append("     [Terminal] = @Terminal, ")
        SQL.Append("     [PageData] = @PageData, ")
        SQL.Append("     [OverrideId] = @OverrideId, ")
        SQL.Append("     [ReversalTxn] = @ReversalTxn ")
        SQL.Append(" WHERE ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command()

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("JournalId", StructObj.JournalId)
        Command.Parameters.AddWithValue("GroupId", StructObj.GroupId)
        Command.Parameters.AddWithValue("TxnCode", StructObj.TxnCode)
        Command.Parameters.AddWithValue("Title", StructObj.Title)
        Command.Parameters.AddWithValue("Status", StructObj.Status)
        Command.Parameters.AddWithValue("StatusDescription", StructObj.StatusDescription)
        Command.Parameters.AddWithValue("ErrCode", StructObj.ErrCode)
        Command.Parameters.AddWithValue("ErrDescription", StructObj.ErrDescription)
        Command.Parameters.AddWithValue("ProcTime", StructObj.ProcTime)
        Command.Parameters.AddWithValue("HostId", StructObj.HostId)
        Command.Parameters.AddWithValue("HostName", StructObj.HostName)
        Command.Parameters.AddWithValue("TraceNo", StructObj.TraceNo)
        Command.Parameters.AddWithValue("SystemDate", StructObj.SystemDate)
        Command.Parameters.AddWithValue("BusinessDate", StructObj.BusinessDate)
        Command.Parameters.AddWithValue("Teller", StructObj.Teller)
        Command.Parameters.AddWithValue("TellerName", StructObj.TellerName)
        Command.Parameters.AddWithValue("BranchId", StructObj.BranchId)
        Command.Parameters.AddWithValue("Supervisor", StructObj.Supervisor)
        Command.Parameters.AddWithValue("SupervisorName", StructObj.SupervisorName)
        Command.Parameters.AddWithValue("Account", StructObj.Account)
        Command.Parameters.AddWithValue("Currency", StructObj.Currency)
        Command.Parameters.AddWithValue("TxnAmount", StructObj.TxnAmount)
        Command.Parameters.AddWithValue("Terminal", StructObj.Terminal)
        Command.Parameters.AddWithValue("PageData", StructObj.PageData)
        Command.Parameters.AddWithValue("OverrideId", StructObj.OverrideId)
        Command.Parameters.AddWithValue("ReversalTxn", StructObj.ReversalTxn)

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

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" DELETE FROM JournalView ")
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

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [JournalId], ")
        SQL.Append("    [GroupId], ")
        SQL.Append("    [TxnCode], ")
        SQL.Append("    [Title], ")
        SQL.Append("    [Status], ")
        SQL.Append("    [StatusDescription], ")
        SQL.Append("    [ErrCode], ")
        SQL.Append("    [ErrDescription], ")
        SQL.Append("    [ProcTime], ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName], ")
        SQL.Append("    [TraceNo], ")
        SQL.Append("    [SystemDate], ")
        SQL.Append("    [BusinessDate], ")
        SQL.Append("    [Teller], ")
        SQL.Append("    [TellerName], ")
        SQL.Append("    [BranchId], ")
        SQL.Append("    [Supervisor], ")
        SQL.Append("    [SupervisorName], ")
        SQL.Append("    [Account], ")
        SQL.Append("    [Currency], ")
        SQL.Append("    [TxnAmount], ")
        SQL.Append("    [Terminal], ")
        SQL.Append("    [PageData], ")
        SQL.Append("    [OverrideId], ")
        SQL.Append("    [ReversalTxn] ")
        SQL.Append(" FROM ")
        SQL.Append("     JournalView ")
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

        If Not IsDBNull(Row(0).Item(0)) Then StructObj.JournalId = Row(0).Item(0)
        If Not IsDBNull(Row(0).Item(1)) Then StructObj.GroupId = Row(0).Item(1)
        If Not IsDBNull(Row(0).Item(2)) Then StructObj.TxnCode = Row(0).Item(2)
        If Not IsDBNull(Row(0).Item(3)) Then StructObj.Title = Row(0).Item(3)
        If Not IsDBNull(Row(0).Item(4)) Then StructObj.Status = Row(0).Item(4)
        If Not IsDBNull(Row(0).Item(5)) Then StructObj.StatusDescription = Row(0).Item(5)
        If Not IsDBNull(Row(0).Item(6)) Then StructObj.ErrCode = Row(0).Item(6)
        If Not IsDBNull(Row(0).Item(7)) Then StructObj.ErrDescription = Row(0).Item(7)
        If Not IsDBNull(Row(0).Item(8)) Then StructObj.ProcTime = Row(0).Item(8)
        If Not IsDBNull(Row(0).Item(9)) Then StructObj.HostId = Row(0).Item(9)
        If Not IsDBNull(Row(0).Item(10)) Then StructObj.HostName = Row(0).Item(10)
        If Not IsDBNull(Row(0).Item(11)) Then StructObj.TraceNo = Row(0).Item(11)
        If Not IsDBNull(Row(0).Item(12)) Then StructObj.SystemDate = Row(0).Item(12)
        If Not IsDBNull(Row(0).Item(13)) Then StructObj.BusinessDate = Row(0).Item(13)
        If Not IsDBNull(Row(0).Item(14)) Then StructObj.Teller = Row(0).Item(14)
        If Not IsDBNull(Row(0).Item(15)) Then StructObj.TellerName = Row(0).Item(15)
        If Not IsDBNull(Row(0).Item(16)) Then StructObj.BranchId = Row(0).Item(16)
        If Not IsDBNull(Row(0).Item(17)) Then StructObj.Supervisor = Row(0).Item(17)
        If Not IsDBNull(Row(0).Item(18)) Then StructObj.SupervisorName = Row(0).Item(18)
        If Not IsDBNull(Row(0).Item(19)) Then StructObj.Account = Row(0).Item(19)
        If Not IsDBNull(Row(0).Item(20)) Then StructObj.Currency = Row(0).Item(20)
        If Not IsDBNull(Row(0).Item(21)) Then StructObj.TxnAmount = Row(0).Item(21)
        If Not IsDBNull(Row(0).Item(22)) Then StructObj.Terminal = Row(0).Item(22)
        If Not IsDBNull(Row(0).Item(23)) Then StructObj.PageData = Row(0).Item(23)
        If Not IsDBNull(Row(0).Item(24)) Then StructObj.OverrideId = Row(0).Item(24)
        If Not IsDBNull(Row(0).Item(25)) Then StructObj.ReversalTxn = Row(0).Item(25)
    End Sub

    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _
                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("    [JournalId], ")
        SQL.Append("    [GroupId], ")
        SQL.Append("    [TxnCode], ")
        SQL.Append("    [Title], ")
        SQL.Append("    [Status], ")
        SQL.Append("    [StatusDescription], ")
        SQL.Append("    [ErrCode], ")
        SQL.Append("    [ErrDescription], ")
        SQL.Append("    [ProcTime], ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName], ")
        SQL.Append("    [TraceNo], ")
        SQL.Append("    [SystemDate], ")
        SQL.Append("    [BusinessDate], ")
        SQL.Append("    [Teller], ")
        SQL.Append("    [TellerName], ")
        SQL.Append("    [BranchId], ")
        SQL.Append("    [Supervisor], ")
        SQL.Append("    [SupervisorName], ")
        SQL.Append("    [Account], ")
        SQL.Append("    [Currency], ")
        SQL.Append("    [TxnAmount], ")
        SQL.Append("    [Terminal], ")
        SQL.Append("    [PageData], ")
        SQL.Append("    [OverrideId], ")
        SQL.Append("    [ReversalTxn] ")
        SQL.Append(" FROM ")
        SQL.Append("     JournalView ")

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
            Dim StructObj As New CJournalViewStruct
            If Not IsDBNull(Row.Item(0)) Then StructObj.JournalId = Row.Item(0)
            If Not IsDBNull(Row.Item(1)) Then StructObj.GroupId = Row.Item(1)
            If Not IsDBNull(Row.Item(2)) Then StructObj.TxnCode = Row.Item(2)
            If Not IsDBNull(Row.Item(3)) Then StructObj.Title = Row.Item(3)
            If Not IsDBNull(Row.Item(4)) Then StructObj.Status = Row.Item(4)
            If Not IsDBNull(Row.Item(5)) Then StructObj.StatusDescription = Row.Item(5)
            If Not IsDBNull(Row.Item(6)) Then StructObj.ErrCode = Row.Item(6)
            If Not IsDBNull(Row.Item(7)) Then StructObj.ErrDescription = Row.Item(7)
            If Not IsDBNull(Row.Item(8)) Then StructObj.ProcTime = Row.Item(8)
            If Not IsDBNull(Row.Item(9)) Then StructObj.HostId = Row.Item(9)
            If Not IsDBNull(Row.Item(10)) Then StructObj.HostName = Row.Item(10)
            If Not IsDBNull(Row.Item(11)) Then StructObj.TraceNo = Row.Item(11)
            If Not IsDBNull(Row.Item(12)) Then StructObj.SystemDate = Row.Item(12)
            If Not IsDBNull(Row.Item(13)) Then StructObj.BusinessDate = Row.Item(13)
            If Not IsDBNull(Row.Item(14)) Then StructObj.Teller = Row.Item(14)
            If Not IsDBNull(Row.Item(15)) Then StructObj.TellerName = Row.Item(15)
            If Not IsDBNull(Row.Item(16)) Then StructObj.BranchId = Row.Item(16)
            If Not IsDBNull(Row.Item(17)) Then StructObj.Supervisor = Row.Item(17)
            If Not IsDBNull(Row.Item(18)) Then StructObj.SupervisorName = Row.Item(18)
            If Not IsDBNull(Row.Item(19)) Then StructObj.Account = Row.Item(19)
            If Not IsDBNull(Row.Item(20)) Then StructObj.Currency = Row.Item(20)
            If Not IsDBNull(Row.Item(21)) Then StructObj.TxnAmount = Row.Item(21)
            If Not IsDBNull(Row.Item(22)) Then StructObj.Terminal = Row.Item(22)
            If Not IsDBNull(Row.Item(23)) Then StructObj.PageData = Row.Item(23)
            If Not IsDBNull(Row.Item(24)) Then StructObj.OverrideId = Row.Item(24)
            If Not IsDBNull(Row.Item(25)) Then StructObj.ReversalTxn = Row.Item(25)
            DatabaseObj.Add(StructObj)
        Next
    End Sub

    Public Sub SearchOrderById(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CJournalViewStruct, _
                                      ByRef JournalViewList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)
        JournalViewList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [JournalId], ")
        SQL.Append("    [GroupId], ")
        SQL.Append("    [TxnCode], ")
        SQL.Append("    [Title], ")
        SQL.Append("    [Status], ")
        SQL.Append("    [StatusDescription], ")
        SQL.Append("    [ErrCode], ")
        SQL.Append("    [ErrDescription], ")
        SQL.Append("    [ProcTime], ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName], ")
        SQL.Append("    [TraceNo], ")
        SQL.Append("    [SystemDate], ")
        SQL.Append("    [BusinessDate], ")
        SQL.Append("    [Teller], ")
        SQL.Append("    [TellerName], ")
        SQL.Append("    [BranchId], ")
        SQL.Append("    [Supervisor], ")
        SQL.Append("    [SupervisorName], ")
        SQL.Append("    [Account], ")
        SQL.Append("    [Currency], ")
        SQL.Append("    [TxnAmount], ")
        SQL.Append("    [Terminal], ")
        SQL.Append("    [PageData], ")
        SQL.Append("    [OverrideId], ")
        SQL.Append("    [ReversalTxn]  ")
        SQL.Append(" FROM ")
        SQL.Append("     JournalView ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" ORDER BY ")
        SQL.Append("     JournalId DESC ")

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
            Dim DBStruct As New CJournalViewStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.JournalId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.GroupId = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.TxnCode = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Title = Row(Idx).Item(3)
            If Not IsDBNull(Row(Idx).Item(4)) Then DBStruct.Status = Row(Idx).Item(4)
            If Not IsDBNull(Row(Idx).Item(5)) Then DBStruct.StatusDescription = Row(Idx).Item(5)
            If Not IsDBNull(Row(Idx).Item(6)) Then DBStruct.ErrCode = Row(Idx).Item(6)
            If Not IsDBNull(Row(Idx).Item(7)) Then DBStruct.ErrDescription = Row(Idx).Item(7)
            If Not IsDBNull(Row(Idx).Item(8)) Then DBStruct.ProcTime = Row(Idx).Item(8)
            If Not IsDBNull(Row(Idx).Item(9)) Then DBStruct.HostId = Row(Idx).Item(9)
            If Not IsDBNull(Row(Idx).Item(10)) Then DBStruct.HostName = Row(Idx).Item(10)
            If Not IsDBNull(Row(Idx).Item(11)) Then DBStruct.TraceNo = Row(Idx).Item(11)
            If Not IsDBNull(Row(Idx).Item(12)) Then DBStruct.SystemDate = Row(Idx).Item(12)
            If Not IsDBNull(Row(Idx).Item(13)) Then DBStruct.BusinessDate = Row(Idx).Item(13)
            If Not IsDBNull(Row(Idx).Item(14)) Then DBStruct.Teller = Row(Idx).Item(14)
            If Not IsDBNull(Row(Idx).Item(15)) Then DBStruct.TellerName = Row(Idx).Item(15)
            If Not IsDBNull(Row(Idx).Item(16)) Then DBStruct.BranchId = Row(Idx).Item(16)
            If Not IsDBNull(Row(Idx).Item(17)) Then DBStruct.Supervisor = Row(Idx).Item(17)
            If Not IsDBNull(Row(Idx).Item(18)) Then DBStruct.SupervisorName = Row(Idx).Item(18)
            If Not IsDBNull(Row(Idx).Item(19)) Then DBStruct.Account = Row(Idx).Item(19)
            If Not IsDBNull(Row(Idx).Item(20)) Then DBStruct.Currency = Row(Idx).Item(20)
            If Not IsDBNull(Row(Idx).Item(21)) Then DBStruct.TxnAmount = Row(Idx).Item(21)
            If Not IsDBNull(Row(Idx).Item(22)) Then DBStruct.Terminal = Row(Idx).Item(22)
            If Not IsDBNull(Row(Idx).Item(23)) Then DBStruct.PageData = Row(Idx).Item(23)
            If Not IsDBNull(Row(Idx).Item(24)) Then DBStruct.OverrideId = Row(Idx).Item(24)
            If Not IsDBNull(Row(Idx).Item(25)) Then DBStruct.ReversalTxn = Row(Idx).Item(25)

            JournalViewList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchByGroupAndUser(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CJournalViewStruct, _
                                      ByRef JournalViewList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)
        JournalViewList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [JournalId], ")
        SQL.Append("    [GroupId], ")
        SQL.Append("    [TxnCode], ")
        SQL.Append("    [Title], ")
        SQL.Append("    [Status], ")
        SQL.Append("    [StatusDescription], ")
        SQL.Append("    [ErrCode], ")
        SQL.Append("    [ErrDescription], ")
        SQL.Append("    [ProcTime], ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName], ")
        SQL.Append("    [TraceNo], ")
        SQL.Append("    [SystemDate], ")
        SQL.Append("    [BusinessDate], ")
        SQL.Append("    [Teller], ")
        SQL.Append("    [TellerName], ")
        SQL.Append("    [BranchId], ")
        SQL.Append("    [Supervisor], ")
        SQL.Append("    [SupervisorName], ")
        SQL.Append("    [Account], ")
        SQL.Append("    [Currency], ")
        SQL.Append("    [TxnAmount], ")
        SQL.Append("    [Terminal], ")
        SQL.Append("    [PageData], ")
        SQL.Append("    [OverrideId], ")
        SQL.Append("    [ReversalTxn]  ")
        SQL.Append(" FROM ")
        SQL.Append("     JournalView ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [GroupId] = ")
        SQL.Append("       @GroupId ")
        SQL.Append("     AND ")
        SQL.Append("     [Teller] = ")
        SQL.Append("       @Teller ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     JournalId DESC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("GroupId", StructObj.GroupId)
        Command.Parameters.AddWithValue("Teller", StructObj.Teller)

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
            Dim DBStruct As New CJournalViewStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.JournalId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.GroupId = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.TxnCode = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Title = Row(Idx).Item(3)
            If Not IsDBNull(Row(Idx).Item(4)) Then DBStruct.Status = Row(Idx).Item(4)
            If Not IsDBNull(Row(Idx).Item(5)) Then DBStruct.StatusDescription = Row(Idx).Item(5)
            If Not IsDBNull(Row(Idx).Item(6)) Then DBStruct.ErrCode = Row(Idx).Item(6)
            If Not IsDBNull(Row(Idx).Item(7)) Then DBStruct.ErrDescription = Row(Idx).Item(7)
            If Not IsDBNull(Row(Idx).Item(8)) Then DBStruct.ProcTime = Row(Idx).Item(8)
            If Not IsDBNull(Row(Idx).Item(9)) Then DBStruct.HostId = Row(Idx).Item(9)
            If Not IsDBNull(Row(Idx).Item(10)) Then DBStruct.HostName = Row(Idx).Item(10)
            If Not IsDBNull(Row(Idx).Item(11)) Then DBStruct.TraceNo = Row(Idx).Item(11)
            If Not IsDBNull(Row(Idx).Item(12)) Then DBStruct.SystemDate = Row(Idx).Item(12)
            If Not IsDBNull(Row(Idx).Item(13)) Then DBStruct.BusinessDate = Row(Idx).Item(13)
            If Not IsDBNull(Row(Idx).Item(14)) Then DBStruct.Teller = Row(Idx).Item(14)
            If Not IsDBNull(Row(Idx).Item(15)) Then DBStruct.TellerName = Row(Idx).Item(15)
            If Not IsDBNull(Row(Idx).Item(16)) Then DBStruct.BranchId = Row(Idx).Item(16)
            If Not IsDBNull(Row(Idx).Item(17)) Then DBStruct.Supervisor = Row(Idx).Item(17)
            If Not IsDBNull(Row(Idx).Item(18)) Then DBStruct.SupervisorName = Row(Idx).Item(18)
            If Not IsDBNull(Row(Idx).Item(19)) Then DBStruct.Account = Row(Idx).Item(19)
            If Not IsDBNull(Row(Idx).Item(20)) Then DBStruct.Currency = Row(Idx).Item(20)
            If Not IsDBNull(Row(Idx).Item(21)) Then DBStruct.TxnAmount = Row(Idx).Item(21)
            If Not IsDBNull(Row(Idx).Item(22)) Then DBStruct.Terminal = Row(Idx).Item(22)
            If Not IsDBNull(Row(Idx).Item(23)) Then DBStruct.PageData = Row(Idx).Item(23)
            If Not IsDBNull(Row(Idx).Item(24)) Then DBStruct.OverrideId = Row(Idx).Item(24)
            If Not IsDBNull(Row(Idx).Item(25)) Then DBStruct.ReversalTxn = Row(Idx).Item(25)

            JournalViewList.Add(DBStruct)
        Next

    End Sub

    Public Sub SearchByTxnCode(ByVal Adapter As IDatabaseAdapter, _
                                      ByVal Obj As CJournalViewStruct, _
                                      ByRef JournalViewList As List(Of Object), _
                                      ByVal Lock As Boolean?) _

        Dim StructObj As CJournalViewStruct = TryCast(Obj, CJournalViewStruct)
        JournalViewList.Clear()

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        If StructObj.MaxRecord > 0 Then SQL.Append(" TOP " & StructObj.MaxRecord)
        SQL.Append("    [JournalId], ")
        SQL.Append("    [GroupId], ")
        SQL.Append("    [TxnCode], ")
        SQL.Append("    [Title], ")
        SQL.Append("    [Status], ")
        SQL.Append("    [StatusDescription], ")
        SQL.Append("    [ErrCode], ")
        SQL.Append("    [ErrDescription], ")
        SQL.Append("    [ProcTime], ")
        SQL.Append("    [HostId], ")
        SQL.Append("    [HostName], ")
        SQL.Append("    [TraceNo], ")
        SQL.Append("    [SystemDate], ")
        SQL.Append("    [BusinessDate], ")
        SQL.Append("    [Teller], ")
        SQL.Append("    [TellerName], ")
        SQL.Append("    [BranchId], ")
        SQL.Append("    [Supervisor], ")
        SQL.Append("    [SupervisorName], ")
        SQL.Append("    [Account], ")
        SQL.Append("    [Currency], ")
        SQL.Append("    [TxnAmount], ")
        SQL.Append("    [Terminal], ")
        SQL.Append("    [PageData], ")
        SQL.Append("    [OverrideId], ")
        SQL.Append("    [ReversalTxn]  ")
        SQL.Append(" FROM ")
        SQL.Append("     JournalView ")
        If Lock Is Nothing Then
            SQL.Append(" WITH (NOLOCK) ")
        ElseIf Lock = True Then
            SQL.Append(" WITH (ROWLOCK) ")
        End If
        SQL.Append(" WHERE ")
        SQL.Append("     ( ")
        SQL.Append("     [TxnCode] = ")
        SQL.Append("       @TxnCode ")
        SQL.Append("     ) ")
        SQL.Append(" ORDER BY ")
        SQL.Append("     JournalId DESC ")

        Dim Command As SqlClient.SqlCommand = Adapter.Command

        Command.CommandType = CommandType.Text
        Command.CommandText = SQL.ToString
        Command.Parameters.AddWithValue("TxnCode", StructObj.TxnCode)

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
            Dim DBStruct As New CJournalViewStruct

            If Not IsDBNull(Row(Idx).Item(0)) Then DBStruct.JournalId = Row(Idx).Item(0)
            If Not IsDBNull(Row(Idx).Item(1)) Then DBStruct.GroupId = Row(Idx).Item(1)
            If Not IsDBNull(Row(Idx).Item(2)) Then DBStruct.TxnCode = Row(Idx).Item(2)
            If Not IsDBNull(Row(Idx).Item(3)) Then DBStruct.Title = Row(Idx).Item(3)
            If Not IsDBNull(Row(Idx).Item(4)) Then DBStruct.Status = Row(Idx).Item(4)
            If Not IsDBNull(Row(Idx).Item(5)) Then DBStruct.StatusDescription = Row(Idx).Item(5)
            If Not IsDBNull(Row(Idx).Item(6)) Then DBStruct.ErrCode = Row(Idx).Item(6)
            If Not IsDBNull(Row(Idx).Item(7)) Then DBStruct.ErrDescription = Row(Idx).Item(7)
            If Not IsDBNull(Row(Idx).Item(8)) Then DBStruct.ProcTime = Row(Idx).Item(8)
            If Not IsDBNull(Row(Idx).Item(9)) Then DBStruct.HostId = Row(Idx).Item(9)
            If Not IsDBNull(Row(Idx).Item(10)) Then DBStruct.HostName = Row(Idx).Item(10)
            If Not IsDBNull(Row(Idx).Item(11)) Then DBStruct.TraceNo = Row(Idx).Item(11)
            If Not IsDBNull(Row(Idx).Item(12)) Then DBStruct.SystemDate = Row(Idx).Item(12)
            If Not IsDBNull(Row(Idx).Item(13)) Then DBStruct.BusinessDate = Row(Idx).Item(13)
            If Not IsDBNull(Row(Idx).Item(14)) Then DBStruct.Teller = Row(Idx).Item(14)
            If Not IsDBNull(Row(Idx).Item(15)) Then DBStruct.TellerName = Row(Idx).Item(15)
            If Not IsDBNull(Row(Idx).Item(16)) Then DBStruct.BranchId = Row(Idx).Item(16)
            If Not IsDBNull(Row(Idx).Item(17)) Then DBStruct.Supervisor = Row(Idx).Item(17)
            If Not IsDBNull(Row(Idx).Item(18)) Then DBStruct.SupervisorName = Row(Idx).Item(18)
            If Not IsDBNull(Row(Idx).Item(19)) Then DBStruct.Account = Row(Idx).Item(19)
            If Not IsDBNull(Row(Idx).Item(20)) Then DBStruct.Currency = Row(Idx).Item(20)
            If Not IsDBNull(Row(Idx).Item(21)) Then DBStruct.TxnAmount = Row(Idx).Item(21)
            If Not IsDBNull(Row(Idx).Item(22)) Then DBStruct.Terminal = Row(Idx).Item(22)
            If Not IsDBNull(Row(Idx).Item(23)) Then DBStruct.PageData = Row(Idx).Item(23)
            If Not IsDBNull(Row(Idx).Item(24)) Then DBStruct.OverrideId = Row(Idx).Item(24)
            If Not IsDBNull(Row(Idx).Item(25)) Then DBStruct.ReversalTxn = Row(Idx).Item(25)

            JournalViewList.Add(DBStruct)
        Next

    End Sub

    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count

        Dim SQL As New Text.StringBuilder
        SQL.Append(" SELECT ")
        SQL.Append("     COUNT(*) ")
        SQL.Append(" FROM ")
        SQL.Append("     JournalView ")

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
            Return "SQLServer_JournalView"
        End Get
    End Property

End Class
