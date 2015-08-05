Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980001
    Inherits BancslinkTxnBase

    Property ExBranchNo As Integer
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("ExBranchNo", ExBranchNo)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim ExBranch As New CExBranch
        Try
            ExBranch.Search(DatabaseFactory, ExBranchNo, False)
            TxnResponse.ExBranchName = ExBranch.ExBranchName
            TxnResponse.ExBranchAddress = "Address Of " & ExBranch.ExBranchName
        Catch ex As Exception
            TxnResponse.ExBranchName = "No Such Branch"
        End Try
    End Sub

    Private TxnResponse As New TxnResp980001
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980001"
        End Get
    End Property

    Public Class TxnResp980001
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property ExBranchName As String
        Property ExBranchAddress As String

        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("ExBranchName", ExBranchName)
            Message.SetValue("ExBranchAddress", ExBranchAddress)

            Return Message
        End Function
    End Class
End Class
