Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow
Imports System.Threading

Public Class CTaskSchedule
    Inherits MarshalByRefObject
    Implements IComponent
    Implements IApplication

    ' Always Alive
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Private _AppConfig As New CTaskScheduleConfig
    Public ReadOnly Property AppConfig As CApplicationConfig Implements IApplication.AppConfig
        Get
            Return _AppConfig
        End Get
    End Property

    Public ReadOnly Property BaseTxnType As Type Implements IApplication.BaseTxnType
        Get
            Return GetType(CTaskBase)
        End Get
    End Property

    Public Sub Start(ByRef AppInfo As CAppConfig.CApplicationInfo) Implements IApplication.Start
        CLog.Info("Initial Tasks")

        ' Print All Tasks
        For Each TaskGroup In _AppConfig.Tasks
            CLog.Info("Task Group   : {0}", TaskGroup.Id)
            CLog.Info("Frequency    : {0}", TaskGroup.Frequency)
            CLog.Info("Repeat       : {0}", TaskGroup.Repeat)

            TaskGroup.TaskFrequency = TaskGroup.GetTaskFrequency()
            If TaskGroup.Frequency Is Nothing Then
                CLog.Warning("Invalid Frequency : {0}", TaskGroup.Frequency)
                TaskGroup.NextRuntime = CTaskScheduleConfig.INVALID_DATETIME

                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_TRANSACTION_PARAMETER, _
                                                    "Invalid Task Frequency Setting. Task {0}, Application {1}", TaskGroup.Id, AppInfo.Id)
            Else
                TaskGroup.NextRuntime = TaskGroup.TaskFrequency.GetNextRuntime(Now())
                CLog.Info("Next Runtime : {0}", TaskGroup.NextRuntime)
            End If

            For Each Task In TaskGroup.Tasks
                CLog.Info("<Task {0}>", Task.TaskId)
                If Not AppConfig.TxnList.ContainsKey(Task.TaskId.ToUpper) Then _
                    Throw New CError.CBusinessException(CError.CErrorCode.INVALID_TRANSACTION_CODE, "Invalid Task {0}", Task.TaskId)
            Next
        Next

        ' Run Task
        Threading.ThreadPool.QueueUserWorkItem(AddressOf StartTasks)
        CLog.Sys("Task Schedule Started")
    End Sub

    Sub StartTasks()
        CLog.Info("Starting")
        While True
            For Each TaskGroup In _AppConfig.Tasks
                If TaskGroup.NextRuntime = CTaskScheduleConfig.INVALID_DATETIME Then
                    Continue For
                End If

                If Now > TaskGroup.NextRuntime And TaskGroup.TaskStatus = CTaskScheduleConfig.CTaskStatus.Stopped Then
                    TaskGroup.TaskStatus = CTaskScheduleConfig.CTaskStatus.Running
                    TaskGroup.LastRuntime = Now()
                    Threading.ThreadPool.QueueUserWorkItem(AddressOf StartTask, TaskGroup)
                End If
            Next
            If StopEvent.IsSet Then
                CLog.Info("Stop to Run Task Schedule")
                Exit While
            End If
            ' Sleep
            Threading.Thread.Sleep(20)
        End While
    End Sub

    Sub StartTask(ByVal TaskInfo As CTaskScheduleConfig.CTaskGroup)
        Dim StopW As New Stopwatch
        StopW.Start()
        AppStatus.IncreaseTxnCount()

        CLog.Sys("Starting Task {0}", TaskInfo.Id)
        Dim Idx As Integer = 1
        For Each Item As CTaskScheduleConfig.CTaskInfo In TaskInfo.Tasks
            CLog.Sys("Start <{0}> : <{1}>", Idx, Item.TaskId)
            Idx += 1
            Dim Task As CTaskBase = Activator.CreateInstance(AppConfig.TxnList.Item(Item.TaskId.ToUpper))
            CLog.Sys("Processing")
            Try
                Dim DatabaseFactory As New CDatabaseFactory(_AppConfig.Database)
                Dim Successful As Boolean = False
                Try
                    Task.Process(DatabaseFactory)
                    Successful = True
                Catch ex As CError.CBusinessException
                    CLog.Err("Task Error {0}, {1}", ex.ErrCode, ex.Message)
                    CLog.Info("Task Group Abort")
                    Exit For
                Catch ex As Exception
                    CLog.Err("Task Error {0}", ex.Message)
                    CLog.Info("Task Group Abort")
                    Exit For
                Finally
                    If Successful = True Then
                        DatabaseFactory.DatabaseAdapter.Commit()
                    Else
                        DatabaseFactory.DatabaseAdapter.Rollback()
                    End If
                    DatabaseFactory.DatabaseAdapter.Close()
                End Try
            Catch ex As Exception

            End Try
        Next
        TaskInfo.TaskStatus = CTaskScheduleConfig.CTaskStatus.Stopped
        ' Set Next Runtime
        Dim Cursor As DateTime = Now
        If TaskInfo.TaskFrequency.Category = CTaskScheduleConfig.CTaskFrequency.CFrequencyCategory.FixedInterval Then Cursor = TaskInfo.LastRuntime
        If TaskInfo.Repeat = CTaskScheduleConfig.CTaskRepeat.Once Then
            TaskInfo.NextRuntime = CTaskScheduleConfig.INVALID_DATETIME
        Else
            TaskInfo.NextRuntime = TaskInfo.TaskFrequency.GetNextRuntime(Cursor)
        End If

        StopW.Stop()
        CLog.Info("Task Finished, Proc Time {0:N3}s", StopW.ElapsedMilliseconds / 1000)
        CLog.Info("Next Runtime : {0}", TaskInfo.NextRuntime)
        AppStatus.DecreaseTxnCount(StopW.ElapsedMilliseconds)
    End Sub

    Private StopEvent As New ManualResetEventSlim(False)

    Public Sub [Stop]() Implements IApplication.Stop
        StopEvent.Set()

        ' TODO
        Dim Sw As New Stopwatch
        Sw.Start()
        While AppStatus.TxnCount > 0
            ' Wait for all transaction finished
            CLog.Info("Transaction Still Processing : {0}", AppStatus.TxnCount)
            Threading.Thread.Sleep(1000)
            If Sw.ElapsedMilliseconds > 50000 Then
                ' Over then 30 seconds
                CLog.Warning("Force Close Application")
                Exit While
            End If
        End While
    End Sub

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "TaskSchedule"
        End Get
    End Property

    Public Property AppStatus As CAppStatus Implements IApplication.AppStatus
End Class
