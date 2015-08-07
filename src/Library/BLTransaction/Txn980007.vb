Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow
Public Class Txn980007
    Inherits BancslinkTxnBase

    Property Branch As String
    Property ProfitAccount As String
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchId", Branch)
        Message.GetValueByKey("ProfitAccount", ProfitAccount)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim CBGL As New CBGLACCOUNTPR
        Dim idx As Integer = 0
        Dim flag As Boolean = True
        While flag
            Try
                CBGL.SearchByACCOUNT(DatabaseFactory, Branch, idx, False)
                If ProfitAccount = CBGL.ACCOUNT.Substring(4, 10) Then
                    TxnResponse.CancelMark = CBGL.CANELMARK
                    CLog.Info("ABD" + CBGL.ACCOUNT.Substring(4, 10))
                    flag = False
                Else
                    TxnResponse.CancelMark = "~"
                End If
                idx += 1
            Catch ex As CError.CBusinessException
                If ex.ErrCode <> CError.CErrorCode.RECORD_NOT_FOUND Then
                    CLog.Err("Retrive Fail {0}: {1}", ex.ErrCode, ex.Message)
                End If
                Exit While
            Catch ex As Exception
                CLog.Err("Retrive Fail {0}", ex.Message)
                Exit While
            End Try
        End While
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980007"
        End Get
    End Property

    Private TxnResponse As New TxnResp980007
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980007
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property CancelMark As String

        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("CancelMark", CancelMark)
            Return Message
        End Function
    End Class

End Class
