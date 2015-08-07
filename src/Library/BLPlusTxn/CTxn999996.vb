Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class CTxn999996
    Inherits TransactionBase
    Property NotificationCategory As Integer
    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("NotificationCateogry", NotificationCategory)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        CLog.Info("Get User({0}) Notifications By Category {1}", UserId, NotificationCategory)
     
        _Response.Notifications.Add(New CTxnResp999996.NotificationList("65673", "Aaron", "1010 : SUP OVERRIDE REQUEST", "10:34:15", "Txn Amount Overlimit"))
        _Response.Notifications.Add(New CTxnResp999996.NotificationList("65671", "Jimmy", "1045 : SUP OVERRIDE REQUEST", "14:19:33", "Force Supervisor Override"))
    End Sub

    Private _Response As New CTxnResp999996
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999996"
        End Get
    End Property
    Public Structure NotificationSummary
        Public UserId As Integer
        Public SupervisorOverride As Integer
        Public Broadcast As Integer

        Sub New(UserId As Integer, SupervisorOverride As Integer, Broadcast As Integer)
            Me.UserId = UserId
            Me.SupervisorOverride = SupervisorOverride
            Me.Broadcast = Broadcast
        End Sub
    End Structure

    Public Class CTxnResp999996
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase

        Public Class NotificationList
            Property NotificationAuthor As String
            Property NotificationAuthorId As Integer
            Property NotificationHeader As String
            Property NotificationDate As String

            Property NotificationDesc As String
            Sub New(AuthorId As Integer, Author As String, Header As String, NoDate As String, Desc As String)
                NotificationAuthorId = AuthorId
                NotificationAuthor = Author
                NotificationDate = NoDate
                NotificationHeader = Header
                NotificationDesc = Desc
            End Sub
        End Class

        Property Notifications As New List(Of NotificationList)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("NotificationList", Notifications)

            Return Message
        End Function


    End Class
End Class

