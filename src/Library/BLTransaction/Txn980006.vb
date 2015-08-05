Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980006
    Inherits BancslinkTxnBase

    Property BranchNo As Integer
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchId", BranchNo)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim Branch As New CBranch
        Try
            Branch.Search(DatabaseFactory, BranchNo, False)
            TxnResponse.BranchCategoryId = Branch.BranchCategoryId
        Catch ex As Exception
            TxnResponse.BranchCategoryId = "No Such Branch"
        End Try
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980006"
        End Get
    End Property

    Private TxnResponse As New TxnResp980006
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980006
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property BranchCategoryId As String

        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("BranchCategoryId", BranchCategoryId)
            Return Message
        End Function
    End Class
End Class
