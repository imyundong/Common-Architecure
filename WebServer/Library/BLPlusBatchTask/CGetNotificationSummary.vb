Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility
Public Class CGetNotificationSummary
    Inherits CTaskBase

    Public Overrides ReadOnly Property Name As String
        Get
            Return "GetNotificationSummary"
        End Get
    End Property

    Public Overrides Sub Process(DatabaseFactory As ServerPlatform.Library.Utility.CDatabaseFactory)
        Dim Notifications As New List(Of NotificationSummary)
        Notifications.Add(New NotificationSummary(65674, 4, 3))
        Notifications.Add(New NotificationSummary(65671, 1, 9))

        CUtility(Of NotificationSummary).WriteToSHM("NotificationSummary", Notifications)
    End Sub

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
End Class
