Imports ServerPlatform.Library.Data
Public Class Txn980013
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)

        Dim idx As Integer = 0
        While True
            Try
                Dim ATMCTL As New CATMCTL
                ATMCTL.SearchByBranch(DatabaseFactory, TellerInfo.BranchId, idx, False)

                TxnResp980013.ATMList.Add(ATMCTL.Clone)
                idx += 1
            Catch ex As Exception
                Exit While
            End Try
        End While

        'Teller.SearchAll(DatabaseFactory, TxnResp999995.OnlineTellerList)
    End Sub

    Private TxnResp980013 As New CTxnResp980013
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResp980013
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 980013
        End Get
    End Property

    Class CTxnResp980013
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property ATMList As New List(Of CATMCTL)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("ATMList", ATMList)
            Return Message
        End Function
    End Class
End Class
