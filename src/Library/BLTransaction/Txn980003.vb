Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn980003
    Inherits BancslinkTxnBase

    Property BRANCHCD As String
    Property BROKERCD As String
    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim BrokerPr As New CBrokerPr
        Try
            BrokerPr.SearchgetBrokername(DatabaseFactory, BRANCHCD, BROKERCD, 0, False)
            TxnResponse.BROKERNAME = BrokerPr.brokername
        Catch ex As Exception
            TxnResponse.BROKERNAME = "No BrokerName"
        End Try
    End Sub

    Private TxnResponse As New TxnResp980003
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property
    Public Class TxnResp980003
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property BROKERNAME As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("BROKERNAME", BROKERNAME)
            Return Message
        End Function
    End Class

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchNo", BRANCHCD)
        Message.GetValueByKey("StockHouseNo", BROKERCD)
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980003"
        End Get
    End Property
End Class
