Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CSystemCore
    Inherits MarshalByRefObject

    Private Shared SyncObject As New Object

    Public Function Start() As CError.CErrorCode
        Try
            ' Initial App Server Platform
            SystemInit()
            ' Start Applications, Get Application Config
            Dim AppConfig As CAppConfig = CAppConfig.GetInstance()

            CLog.Info("Id                   Category                            Config                                       Path")
            CLog.Info("-------------------- ----------------------------------- -------------------------------------------- ---------------")
            For Each Item As CAppConfig.CApplicationInfo In AppConfig.ApplicationConfig.Appliations
                CLog.Info("{0, -20:G} {1, -35:G} {2, -44:G} {3, -15:G}", Item.Id, Item.Category, Item.ConfigFile, Item.TxnPath)
            Next
            CLog.Info("-------------------- ----------------------------------- -------------------------------------------- ---------------")
            CLog.Info("Totally {0} Applications", AppConfig.ApplicationConfig.Appliations.Count)

            Dim Total As Integer = AppConfig.ApplicationConfig.Appliations.Count
            Dim Started As Integer = 0

            For Each Item As CAppConfig.CApplicationInfo In AppConfig.ApplicationConfig.Appliations
                CLog.Sys("Start Application {{{0}}}", Item.Id)
                Try
                    Dim AppState As CAppState
                    If ApplicationList.ContainsKey(Item.Id) Then
                        AppState = ApplicationList.Item(Item.Id)
                        CLog.Info("Application Status : {0}", AppState.AppStatus.Status)

                        If (AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Stopped Or _
                            AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Failed) Then
                            CLog.Warning("Invalid Application Status, Application Start Cancelled")

                            If AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Starting Then
                                Throw New CError.CBusinessException(CError.CErrorCode.SYSTEM_IS_STARTING, _
                                                                    "System Is Starting, Please Wait...")
                            ElseIf AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Started Then
                                Throw New CError.CBusinessException(CError.CErrorCode.SYSTEM_ALREADY_STARTED, _
                                                                    "System Already Started")
                            Else
                                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_APPLICATION_STATUS, _
                                                                    "Invalid Application Status")
                            End If

                            If Not AppState.Domain Is Nothing Then
                                ' Unload Domain
                                CLog.Info("Unload Domain {0}", AppState.Domain.FriendlyName)
                                AppDomain.Unload(AppState.Domain)
                            End If

                        End If

                    Else
                        AppState = New CAppState(New CAppStatus, Nothing)
                        ApplicationList.Add(Item.Id, AppState)
                    End If

                    StartApplication(Item, AppState)
                    Started += 1
                Catch ex As CError.CBusinessException
                    CLog.Err("Application Start Failed {0} {{{1}}}", ex.ErrCode, ex.Message)
                Catch ex As Exception
                    CLog.Err("Application Start Failed {{{0}}}", ex.Message)
                End Try
            Next

            If Started = 0 Then
                Throw New CError.CBusinessException(CError.CErrorCode.APPLICATION_PLATFORM_START_FAIL, "None of The Application Start Successful")
            ElseIf Started <> Total Then
                CLog.Warning("Platform Ready, Partially Application Started")
            Else
                CLog.Info("Platform Ready")
            End If
        Catch ex As CError.CBusinessException
            CLog.Err("Start Application Platform Fail {0}", ex)
            Return ex.ErrCode
        Catch ex As Exception
            CLog.Err("Start Application Platform Fail {0}", ex)
            Return CError.CErrorCode.INTERNAL_ERROR
        End Try

        Return CError.CErrorCode.SUCCESSFUL
    End Function

    Property ApplicationList As New Generic.Dictionary(Of String, CAppState)

    Public Sub StartApplication(ByVal AppId As String)
        Dim AppConfig As CAppConfig = CAppConfig.GetInstance()
        Dim AppState As CAppState = ApplicationList.Item(AppId)

        For Each AppInfo In AppConfig.ApplicationConfig.Appliations
            If AppInfo.Id = AppId Then
                If AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Stopped OrElse AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Failed Then
                    StartApplication(AppInfo, AppState)
                End If
            End If
        Next

    End Sub
    Public Sub StartApplication(ByVal AppInfo As CAppConfig.CApplicationInfo, _
                                ByVal AppState As CAppState)
        ' Starting
        Dim Domain As String = AppInfo.Id
        CLog.Sys("Create App Domian {{{0}}}", Domain)
        'Dim AppServerDomain As AppDomain = AppDomain.CreateDomain(Domain, Nothing, CUtility.ServerPath(""), Nothing, False)
        Dim SubDomain As AppDomain = AppDomain.CreateDomain(Domain, Nothing, CUtility.ServerPath(""), Nothing, False)
        AppState.Domain = SubDomain

        Try
            Dim MyType As Type = GetType(CAppLoader)

            CLog.Sys("Reflecting Application Loader")
            Dim ApplicationLoader As CAppLoader =
                SubDomain.CreateInstanceFromAndUnwrap(MyType.Assembly.CodeBase, MyType.FullName)
            CLog.Sys("Start Application {{{0}}}, {{{1}}}", AppInfo.Id, AppInfo.Category)

            Dim AppConfig As CAppConfig = CAppConfig.GetInstance()
            ApplicationLoader.Start(AppInfo, AppConfig.CachedComponentList, AppState.AppStatus)

            If AppState.AppStatus.ErrCode <> CError.CErrorCode.SUCCESSFUL Then
                Throw New CError.CBusinessException(AppState.AppStatus.ErrCode, "Start Application Fail")
            End If

            AppState.AppLoader = ApplicationLoader
        Catch ex As CError.CBusinessException
            AppState.AppStatus.ErrCode = ex.ErrCode
            AppState.Domain = Nothing
            AppDomain.Unload(SubDomain)
            Throw ex
        Catch ex As Exception
            AppState.AppStatus.ErrCode = CError.CErrorCode.INTERNAL_ERROR
            AppState.Domain = Nothing
            AppDomain.Unload(SubDomain)
            Throw ex
        End Try
    End Sub

    Public Sub StopApplication(ByVal Id As String)
        If Not ApplicationList.ContainsKey(Id) Then
            CLog.Err("Application {0} Is Not Available", Id)
            Exit Sub
        End If

        Dim AppState As CAppState = ApplicationList.Item(Id)
        AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Stopping
        CLog.Info("Stop Application : {0}", Id)
        Try
            AppState.AppLoader.Stop()
        Catch ex As Exception
            CLog.Warning(ex.ToString)
        Finally
            CLog.Info("Unload Domain")
            Try
                AppDomain.Unload(AppState.Domain)
            Catch ex As Exception
            Finally
                AppState.AppStatus.Status = CAppStatus.CRuntimeStatus.Stopped
            End Try

        End Try

    End Sub

    Private Sub SystemInit()
        ' Step 1 : Load Configuration
        Dim AppConfig As CAppConfig = CAppConfig.GetInstance()

        If CEnviroment.Status = CSystemStatus.Started Then
            Exit Sub
        End If

        If CEnviroment.Status = CSystemStatus.Failed Then
            CLog.Warning("Last Start Failed, Try to Restart")
        End If

        ' Step 2 : Initial Log
        CLog.Init(AppConfig.LogInfo)
        CLog.Sys("System Starting...")
        CLog.Info("Status : {0}", CEnviroment.Status)

        CLog.Info("Log Initial Successful")
        CLog.Debug("Log Size", AppConfig.LogInfo.LogSize)
        CLog.Debug("Path", AppConfig.LogInfo.LogPath)
        CLog.Debug("Log Count", AppConfig.LogInfo.LogCount)
        CLog.Debug("Level", AppConfig.LogInfo.LogLevel)

        ' Step 3 : Load Platform Component

        CLog.Info("Loading Platform Component")

        ' Clearing Component Cache
        AppConfig.CachedComponentList.Clear()

        Try
            ' Loading Communication Component
            CLog.Sys("Loading Communication Component")
            Dim Count As Integer = LoadSystemComponent(AppConfig.PlatformComponent.Communication)
            If Count = 0 Then CLog.Warning("0 Communication Component Found")
            ' Loading Application Component
            CLog.Sys("Loading Application Component")
            Count = LoadSystemComponent(AppConfig.PlatformComponent.Application)
            If Count = 0 Then CLog.Warning("0 Application Component Found")
            ' Loading Database Component
            CLog.Sys("Loading Database Component")
            Count = LoadSystemComponent(AppConfig.PlatformComponent.Database)
            If Count = 0 Then CLog.Warning("0 Database Component Found")
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function LoadSystemComponent(ByVal ComponentInfo As CAppConfig.CComponentInfo) As Integer
        Dim AppConfig As CAppConfig = CAppConfig.GetInstance()

        Dim Idx As Integer = 0
        For Each Item As CAppConfig.CComponentDetail In ComponentInfo.ComponentList
            Try
                Dim CommType As Type = CUtility.ComponentLoader(ComponentInfo.ComponentPath, _
                                         Item.Assembly, Item.Category, GetType(ICommunication))
                Dim FullName As String = Item.Assembly.ToUpper + "." + Item.Category.ToUpper
                CLog.Sys("Add {{{0}}} in Component Cache", FullName)
                AppConfig.CachedComponentList.Add(FullName, CommType)
                Idx += 1
            Catch ex As Exception
                CLog.Warning(ex.Message)
            End Try
        Next

        Return Idx
    End Function

End Class
