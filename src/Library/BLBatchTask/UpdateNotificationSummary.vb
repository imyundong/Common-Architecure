Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility

Public Class UpdateNotificationSummary
    Inherits CTaskBase
    Public Overrides ReadOnly Property Name As String
        Get
            Return "UpdateNotificationSummary"
        End Get
    End Property

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        CLog.Info("Update IBD Notifiaction Infomation in Notification Summary Table")
        Dim NotificationSummary As New CNotificationSummary

        Dim Mapping As New CNotificationSummaryMapping
        NotificationSummary.SearchAll(DatabaseFactory, Mapping.NotificationList)
        Mapping.Ticks = Now.Ticks

        CLog.Info("Total {0} IBD Notification Found", Mapping.NotificationList.Count)

        ' Get Marquee from Database
        Dim Marquee As New CMarquee
        Marquee.Search(DatabaseFactory, 1, Nothing)
        Mapping.Marquee = Marquee.Content

        CLog.Info("Update IBD Notification in Shared Memory")
        Try
            GenericType.CUtility(Of CNotificationSummaryMapping).WriteToSHM("NotificationSummaryMapping", Mapping)

            CLog.Info("Update Successful")
        Catch ex As Exception
            CLog.Err(ex.Message)
        End Try

        CLog.Info("Retrieve Branch Status")
     
        Try
            Dim Branch As New CBranch
            Dim BranchList As New List(Of CBranch)
            Branch.SearchAll(DatabaseFactory, BranchList)

            Dim StructureBranchList As New List(Of CBranchStatus)
            For Each Branch In BranchList
                Dim Status As Boolean = False
                If (Not String.IsNullOrEmpty(Branch.BranchStatus)) AndAlso Branch.BranchStatus.Trim = "OPEN" Then
                    Status = True
                End If
                StructureBranchList.Add(New CBranchStatus(Branch.BranchId, Status))
            Next
            CLog.Info("Update Branch Status in Shared Memory")
            CUtility(Of CBranchStatus).WriteToSHM("BancslinkBranchStatus", StructureBranchList)
            CLog.Info("Update Successful")
        Catch ex As Exception
            CLog.Err(ex.Message)
        End Try
    End Sub

    <Serializable>
    Public Class CNotificationSummaryMapping
        Property Ticks As Long
        Property Marquee As String = ""
        Property NotificationList As New List(Of CNotificationSummary)
    End Class

    Public Structure CBranchStatus
        Property BranchId As Integer
        Property IsOpened As Boolean

        Public Sub New(BranchId As Integer, IsOpened As Boolean)
            Me.BranchId = BranchId
            Me.IsOpened = IsOpened
        End Sub
    End Structure
End Class
