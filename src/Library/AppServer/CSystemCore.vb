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

            CLog.Info("Id         Category                            Config                                   Path")
            CLog.Info("---------- ----------------------------------- ---------------------------------------- ---------------")
            For Each Item As CAppConfig.CApplicationInfo In AppConfig.ApplicationConfig.Appliations
                CLog.Info("{0, -10:G} {1, -24:G} {2, -40:G} {3, -15:G}", Item.Id, Item.Category, Item.ConfigFile, Item.TxnPath)
            Next
            CLog.Info("---------- ----------------------------------- ---------------------------------------- ---------------")
            CLog.Info("Totally {0} Applications", AppConfig.ApplicationConfig.Appliations.Count)

            For Each Item As CAppConfig.CApplicationInfo In AppConfig.ApplicationConfig.Appliations
                CLog.Sys("Start Application {{{0}}}", Item.Id)
                StartApplication(Item)
            Next
        Catch ex As CError.CBusinessException
            Return ex.ErrCode
        Catch ex As Exception
            Return CError.CErrorCode.INTERNAL_ERROR
        End Try

        Return CError.CErrorCode.SUCCESSFUL

    End Function

    Public Sub StartApplication(ByVal AppInfo As CAppConfig.CApplicationInfo)
        ' Starting
        Dim Domain As String = AppInfo.Id
        CLog.Sys("Create App Domian {{{0}}}", Domain)
        'Dim AppServerDomain As AppDomain = AppDomain.CreateDomain(Domain, Nothing, CUtility.ServerPath(""), Nothing, False)
        Dim AppServerDomain As AppDomain = AppDomain.CreateDomain(Domain, Nothing, CUtility.ServerPath(""), Nothing, False)
        Try
            Dim MyType As Type = GetType(CAppLoader)

            CLog.Sys("Reflecting Application Loader")
            Dim ApplicationLoader As CAppLoader =
                AppServerDomain.CreateInstanceFromAndUnwrap(MyType.Assembly.CodeBase, MyType.FullName)
            ' Passing Internal Communication Object
            CLog.Sys("Start Application {{{0}}}, {{{1}}}", AppInfo.Id, AppInfo.Category)

            Dim AppConfig As CAppConfig = CAppConfig.GetInstance()
            Dim ErrCode As CError.CErrorCode = ApplicationLoader.Start(AppInfo, AppConfig.CachedComponentList)
            If ErrCode <> CError.CErrorCode.SUCCESSFUL Then
                Throw New CError.CBusinessException(ErrCode, "Start Application Fail")
            End If

        Catch ex As Exception
            AppDomain.Unload(AppServerDomain)
            Throw ex
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
