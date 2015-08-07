Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

<Serializable>
Public Class CAppLoader
    Inherits MarshalByRefObject
    Private Application As IApplication
    Public Sub Start(ByVal AppInfo As CAppConfig.CApplicationInfo, _
                          ByVal SystemComponents As Dictionary(Of String, Type), _
                          ByRef AppStatus As CAppStatus)
        AppStatus.Status = CAppStatus.CRuntimeStatus.Starting
        ' Start Application
        If Not SystemComponents.ContainsKey(AppInfo.Category.ToUpper) Then
            AppStatus.ErrCode = CError.CErrorCode.INVALID_APPLICATION_CATEGORY
            Exit Sub
        End If

        Application = Activator.CreateInstance(SystemComponents.Item(AppInfo.Category.ToUpper))
        Dim AppConfig As CApplicationConfig = Application.AppConfig

        ' Load Application Config
        Dim ConfigPath As String = CUtility.ServerPath(AppInfo.ConfigFile)
        AppConfig.Load(ConfigPath)

        CLog.Init(AppConfig.LogInfo)
        CLog.Info("Starting")

        CLog.Info("App Id      : {0}", AppInfo.Id)
        CLog.Info("Category    : {0}", AppInfo.Category)
        CLog.Info("Config Path : {0}", AppInfo.ConfigFile)
        CLog.Info("Txn Path    : {0}", AppInfo.TxnPath)

        CLog.Sys("Loading Transaction")
        Dim ComponentList As Generic.Dictionary(Of String, Type) = LoadTxnComponent(AppInfo.TxnPath)
        If ComponentList Is Nothing OrElse ComponentList.Count = 0 Then
            CLog.Err("Load Txn Component Fail")
            AppStatus.ErrCode = CError.CErrorCode.LOAD_TXN_COMPONENT_FAIL
            Exit Sub
        End If

        For Each Item As Generic.KeyValuePair(Of String, Type) In SystemComponents
            If Not ComponentList.ContainsKey(Item.Key) Then ComponentList.Add(Item.Key, Item.Value)
        Next

        ' Building Application Component/Txn List
        If AppConfig.ComponentList Is Nothing Then AppConfig.ComponentList = New Generic.Dictionary(Of String, Type)
        If AppConfig.TxnList Is Nothing Then AppConfig.TxnList = New Generic.Dictionary(Of String, Type)
        If AppConfig.DatabaseAccessList Is Nothing Then AppConfig.TxnList = New Generic.Dictionary(Of String, Type)

        For Each Item As Generic.KeyValuePair(Of String, Type) In ComponentList
            Try
                If ContainBaseType(Item.Value, Application.BaseTxnType) Then
                    AppConfig.TxnList.Add(Item.Key.Remove(0, Item.Key.IndexOf("."c) + 1), Item.Value)
                ElseIf Item.Value.GetInterfaces.Contains(GetType(IDatabaseAccess)) Then
                    Dim Name() As String = Item.Key.Split("."c)
                    Dim ShortName As String = Name(Name.Length - 1)

                    AppConfig.DatabaseAccessList.Add(ShortName, Item.Value)
                Else

                    AppConfig.ComponentList.Add(Item.Key, Item.Value)
                End If
            Catch ex As Exception
            End Try
        Next

        CLog.Info("Transaction Count  : {0}", AppConfig.TxnList.Count)
        CLog.Info("Component Count    : {0}", AppConfig.ComponentList.Count)
        If AppConfig.TxnList.Count = 0 Then
            AppStatus.ErrCode = CError.CErrorCode.TXN_COMPONENT_NOT_FOUND
            Exit Sub
        End If
        CLog.Info("Transaction/Components Loaded")
        CLog.Sys("Start Application...")
        Try
            Application.AppStatus = AppStatus
            Application.Start(AppInfo)
        Catch ex As CError.CBusinessException
            CLog.Err("Application Start Fail, Due to {0}", ex.Message)
            AppStatus.ErrCode = ex.ErrCode
        Catch ex As Exception
            CLog.Err(ex.Message)
            AppStatus.ErrCode = CError.CErrorCode.INTERNAL_ERROR
        End Try

        AppStatus.Status = CAppStatus.CRuntimeStatus.Started
    End Sub

    ' Load Txn Component
    Private Function LoadTxnComponent(ByVal Path As String) As Generic.Dictionary(Of String, Type)
        ' Get Files From Existing Folder
        Dim Files As String() = IO.Directory.GetFiles(CUtility.ServerPath(Path), "*.dll")
        Dim TxnList As New Generic.Dictionary(Of String, Type)
        ' Loading Transactions
        For Each File As String In Files
            Try
                Dim FileInfo As New IO.FileInfo(File)
                Dim ComponentList As Generic.Dictionary(Of String, Type) = _
                    CUtility.ComponentLoader(FileInfo.DirectoryName, FileInfo.Name.Substring(0, FileInfo.Name.Length - 4))

                For Each Item In ComponentList
                    If Not TxnList.ContainsKey(Item.Key) Then TxnList.Add(Item.Key, Item.Value)
                Next
            Catch ex As Exception

            End Try
        Next

        Return TxnList
    End Function

    Private Function ContainBaseType(ByVal SourceType As Type, ByVal DestType As Type) As Boolean
        If SourceType.BaseType Is Nothing Then
            Return False
        End If

        If SourceType.BaseType = DestType Then
            Return True
        Else
            Return ContainBaseType(SourceType.BaseType, DestType)
        End If
    End Function

    Public Sub [Stop]()
        Application.Stop()
    End Sub

End Class

