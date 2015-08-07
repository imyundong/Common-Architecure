Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980008
    Inherits BancslinkTxnBase

    Property BranchNo As Integer
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchNo", BranchNo)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim TBranch As New CBranch
        Try
            TBranch.SearchByBrahchIdGetName(DatabaseFactory, BranchNo, Nothing, False)
            TxnResponse.BranchName = TBranch.BranchName

        Catch ex As Exception
            TxnResponse.BranchName = "No Such Branch"
        End Try
    End Sub

    Private TxnResponse As New TxnResp980008
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980008"
        End Get
    End Property

    Public Class TxnResp980008
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property BranchName As String


        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("BranchName", BranchName)


            Return Message
        End Function
    End Class
End Class
