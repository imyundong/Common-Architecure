Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn980004
    Inherits BancslinkTxnBase
    Property BankCode As String
    Property BranchCode As String
    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim othb As New COTHB
        Try
            othb.SearchgetBranchFullName(DatabaseFactory, BankCode, BranchCode, 0, False)
            TxnResponse.BranchFullName = othb.BranchFullName
            TxnResponse.BranchShortName = othb.BranchShortName
        Catch ex As Exception
            TxnResponse.BranchFullName = ""
            TxnResponse.BranchShortName = ""
        End Try
    End Sub

    Private TxnResponse As New TxnResp980004
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980004
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property BranchFullName As String
        Property BranchShortName As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("BranchFullName", BranchFullName)
            Message.SetValue("BranchShortName", BranchShortName)
            Return Message
        End Function
    End Class

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchNo", BankCode)
        Message.GetValueByKey("BranchCode", BranchCode)
    End Sub
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980004"
        End Get
    End Property
End Class
