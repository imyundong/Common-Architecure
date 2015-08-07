Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class CTxn999997
    Inherits TransactionBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Notifications() = CUtility(Of NotificationSummary).ReadFromSHM("NotificationSummary")

        For Each Notification As NotificationSummary In Notifications
            If UserId = Notification.UserId Then
                _Response.Notifications.Add(New CTxnResp999997.NotificationSummary(1, "Supervisor Override", Notification.SupervisorOverride))
                _Response.Notifications.Add(New CTxnResp999997.NotificationSummary(2, "Broadcast", Notification.Broadcast))
            End If
        Next
    End Sub

    Private _Response As New CTxnResp999997
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999997"
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

    Public Class CTxnResp999997
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase

        Public Class NotificationSummary
            Property NotificationTitle As String
            Property NotificationCount As Integer
            Property NotificationCategory As Integer

            Sub New(Category As String, Title As String, Count As Integer)
                NotificationCategory = Category
                NotificationTitle = Title
                NotificationCount = Count
            End Sub
        End Class

        Property Notifications As New List(Of NotificationSummary)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("Notifications", Notifications)

            Return Message
        End Function


    End Class
End Class

