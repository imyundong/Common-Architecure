Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980012
    Inherits BancslinkTxnBase
    
    Property Branch As String
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchID", Branch)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim BranchST As New CBranchST
        Try
            BranchST.SearchByAllStatus(DatabaseFactory, BranchID, 0, False)
            TxnResponse.UnionBalancingStatus = BranchST.UNIONBALANCINGSTATUS
            TxnResponse.BalancingStatus = BranchST.BALANCINGSTATUS
            TxnResponse.BusinessDate = BranchST.BUSINESSDATE
            TxnResponse.RemittanceStatus = BranchST.REMITTANCESTATUS
            TxnResponse.FrxBalancingStatus = BranchST.FRXBALANCINGSTATUS

            CLog.Info("ABD" + TxnResponse.UnionBalancingStatus)
        Catch ex As Exception
            TxnResponse.UnionBalancingStatus = "~"
            TxnResponse.BalancingStatus = "~"
            TxnResponse.BusinessDate = "~"
            TxnResponse.RemittanceStatus = "~"
            TxnResponse.FrxBalancingStatus = "~"
            CLog.Info("ABD" + "1")
        End Try
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980012"
        End Get
    End Property

    Private TxnResponse As New TxnResp980012
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980012
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property UnionBalancingStatus As String
        Property BalancingStatus As String
        Property BusinessDate As String
        Property RemittanceStatus As String
        Property FrxBalancingStatus As String

        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("UnionBalancingStatus", UnionBalancingStatus)
            Message.SetValue("BalancingStatus", BalancingStatus)
            Message.SetValue("BusinessDate", BusinessDate)
            Message.SetValue("RemittanceStatus", RemittanceStatus)
            Message.SetValue("FrxBalancingStatus", FrxBalancingStatus)


            Return Message
        End Function
    End Class
End Class
