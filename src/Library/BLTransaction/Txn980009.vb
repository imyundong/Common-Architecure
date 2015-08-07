Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980009
    Inherits BancslinkTxnBase

    Property TitleId As String
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("TITLEID", TitleId)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim BGLQuotaPR As New CBGLACCOUNTQUOTAPR
        Try
            BGLQuotaPR.Search(DatabaseFactory, TitleId, False)
            TxnResponse.LimitAmount = BGLQuotaPR.LIMITAMOUNT
            CLog.Info("ABD" + TxnResponse.LimitAmount)
        Catch ex As Exception
            TxnResponse.LimitAmount = "~"
            CLog.Info("ABD" + "1")
        End Try
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980009"
        End Get
    End Property

    Private TxnResponse As New TxnResp980009
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980009
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property LimitAmount As String

        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("LIMITAMOUNT", LimitAmount)
            Return Message
        End Function
    End Class
End Class
