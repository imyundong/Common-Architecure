Imports ServerPlatform.Library.Data

Public Class CTxn099999
    Inherits TransactionBase

    Property GroupId As String
    Property OriginalUser As Integer

    Property OriginalTxnCode As String


    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("GroupId", GroupId)
        Message.GetValueByKey("OriginalTxnCode", OriginalTxnCode)
        Message.GetValueByKey("OriginalUser", OriginalUser)
    End Sub
    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim JournalView As New CJournalView
        Try
            Dim Idx As Integer
            While (True)
                If Not String.IsNullOrEmpty(GroupId) Then
                    JournalView.SearchByGroupAndUser(DatabaseFactory, GroupId, OriginalUser, Idx, Nothing, 20)
                ElseIf Not String.IsNullOrEmpty(OriginalTxnCode) Then
                    JournalView.SearchByTxnCode(DatabaseFactory, OriginalTxnCode, Idx, Nothing, 20)
                Else
                    JournalView.SearchOrderById(DatabaseFactory, Idx, Nothing, 20)
                End If
                JournalView.PageData = ""
                _Response.JournalItems.Add(JournalView.Clone)
                Idx += 1
            End While
        Catch ex As Exception

        End Try
    
    End Sub

    Private _Response As New CTxnResp099999
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "099999"
        End Get
    End Property

    Public Class CTxnResp099999
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property JournalItems As New List(Of CJournalView)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("JournalItems", JournalItems)

            Return Message
        End Function

    End Class
End Class

