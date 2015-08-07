Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980010
    Inherits BancslinkTxnBase
    Property CurrentDate As String
    Property ISOperDate As String
    Property SearchType As String
    Private TxnResponse As New TxnResp980010

    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("CurrentDate", CurrentDate)
        Message.GetValueByKey("ISOperDate", ISOperDate)
        Message.GetValueByKey("SearchType", SearchType)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim TOperDate As New COperdate
        Dim Result As String
        Try
            If SearchType = "checkDate" Then
                TOperDate.SearchByOperDate(DatabaseFactory, CurrentDate, ISOperDate, 0, False)
                Result = TOperDate.OPDATE
            ElseIf SearchType = "getNext" Then
                TOperDate.SearchNextDateByOperDate(DatabaseFactory, CurrentDate, ISOperDate, 0, False)
                Result = TOperDate.OPDATE
            ElseIf SearchType = "getPrev" Then
                TOperDate.SearchPreDateByOperDate(DatabaseFactory, CurrentDate, ISOperDate, 0, False)
                Result = TOperDate.OPDATE
            Else
                Result = ""
                TxnResponse.Message = "SearchType Error"
            End If

            TxnResponse.CurrentDate = Result
        Catch ex As Exception
            TxnResponse.Message = ex.Message
            TxnResponse.CurrentDate = ""
        End Try
    End Sub

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980010"
        End Get
    End Property

    Public Class TxnResp980010
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property CurrentDate As String
        Property Message As String

        Public Overrides Function Encode() As Library.Data.CMessage '返回給Screen之資料
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("CurrentDate", CurrentDate)
            Return Message
        End Function
    End Class
End Class
