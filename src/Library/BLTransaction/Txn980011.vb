Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class Txn980011
    Inherits BancslinkTxnBase

    Property BusinessDate As String

    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("BusinessDate", BusinessDate)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim MultiTxnSeq As New CMultiTxnSeq

        Dim DateValue As String() = BusinessDate.Split("/")
        Try
            MultiTxnSeq.SearchByTellerBusinessDate(DatabaseFactory, UserId, New Date(DateValue(2), DateValue(1), DateValue(0)), 0, False)
            TxnResponse.MultiSeq = MultiTxnSeq.MULTISEQ + 1
        Catch ex As CError.CBusinessException
            CLog.Info("Exception : {0}", ex.Message)
            TxnResponse.MultiSeq = 1
        End Try

    End Sub

    Private TxnResponse As New TxnResp980011
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980011"
        End Get
    End Property

    Public Class TxnResp980011
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property MultiSeq As Integer


        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("MultiSeq", MultiSeq.ToString("0000"))


            Return Message
        End Function
    End Class
End Class
