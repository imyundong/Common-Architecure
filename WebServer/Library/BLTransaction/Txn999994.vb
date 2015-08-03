Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
''' <summary>
''' Get Online Supervisor List
''' </summary>
''' <remarks></remarks>
Public Class Txn999994
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Terminal As New CTerminal
        CLog.Info("Get Terminal Information")
        Terminal.Search(DatabaseFactory, TellerInfo.TerminalId, False)
        CLog.Info("Terminal Branch : {0}, {1}", TellerInfo.BranchId, Terminal.BranchId)

        Dim idx As Integer = 0
        Dim OnlineSupervisor As New CSignOnSupervisor
        While True
            Try
                OnlineSupervisor.SearchByCurrentBranchAndTellerId(DatabaseFactory, Terminal.BranchId, TellerInfo.TellerId, idx, False)
                idx += 1
                TxnResp999994.OnlineSupervisorList.Add(OnlineSupervisor.Clone)
            Catch ex As Exception
                CLog.Info("Totally {0} Teller Found", idx)
                Exit While
            End Try
        End While

        'Teller.SearchAll(DatabaseFactory, TxnResp999994.OnlineTellerList)
    End Sub

    Private TxnResp999994 As New CTxnResp999994
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResp999994
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999994
        End Get
    End Property

    Class CTxnResp999994
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property OnlineSupervisorList As New List(Of CSignOnSupervisor)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("OnlineSupervisorList", OnlineSupervisorList)
            Return Message
        End Function
    End Class
End Class
