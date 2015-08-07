Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn970000
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Try
            Dim Map As CNotificationSummaryMapping = _
                GenericType.CUtility(Of CNotificationSummaryMapping).ReadFromSHM("NotificationSummaryMapping")
            TxnResponse.Ticks = Map.Ticks

            For Each Item In Map.NotificationList
                If Item.TellerId.ToUpper = UserId.ToUpper Then
                    TxnResponse.IBDNotificationCount = Item.IBDNotificationCount
                    TxnResponse.PassThruNotificationCount = Item.PassThruNotificationCount
                    TxnResponse.BroadcastNotificationCount = Item.BroadcastNotificationCount
                End If
            Next

            TxnResponse.Marquee = Map.Marquee

            Dim BranchStatus() As CBranchStatus = CUtility(Of CBranchStatus).ReadFromSHM("BancslinkBranchStatus")
            For Each Item As CBranchStatus In BranchStatus
                If TellerInfo.BranchId = Item.BranchId Then
                    TxnResponse.BranchStatus = Item.IsOpened
                End If
            Next

        Catch ex As Exception
            CLog.Info("Retrieve Shared Memory Failed : {0}", ex.Message)
        End Try
    End Sub

    Private TxnResponse As New TxnResp980000

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "970000"
        End Get
    End Property

    <Serializable>
    Public Class TxnResp980000
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property Ticks As Long = 0
        Property IBDNotificationCount As Integer = 0
        Property PassThruNotificationCount As Integer = 0
        Property BroadcastNotificationCount As Integer = 0
        Property BranchStatus As Boolean
        Property Marquee As String = ""

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("Ticks", Ticks)
            Message.SetValue("IBDNotificationCount", IBDNotificationCount)
            Message.SetValue("PassThruNotificationCount", PassThruNotificationCount)
            Message.SetValue("BroadcastNotificationCount", BroadcastNotificationCount)
            Message.SetValue("BranchStatus", BranchStatus)
            Message.SetValue("Marquee", Marquee)
            Return Message
        End Function
    End Class

    Public Structure CBranchStatus
        Property BranchId As Integer
        Property IsOpened As Boolean

        Public Sub New(BranchId As Integer, IsOpened As Boolean)
            Me.BranchId = BranchId
            Me.IsOpened = IsOpened
        End Sub
    End Structure

    Public Class CNotificationSummaryMapping
        Property Ticks As Long
        Property Marquee As String = ""
        Property NotificationList As New List(Of CNotificationSummary)
    End Class
End Class
