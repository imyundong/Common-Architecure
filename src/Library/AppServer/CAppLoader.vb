Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

<Serializable>
Public Class CAppLoader
    Inherits MarshalByRefObject

    Property Reference As CReferenceObject
    Public Function Start(ByVal AppInfo As CAppConfig.CApplicationInfo, ByVal CachedComponent As Dictionary(Of String, Type)) As CError.CErrorCode
        ' Starting Application
        If Not CachedComponent.ContainsKey(AppInfo.Category.ToUpper) Then
            Return CError.CErrorCode.INVALID_APPLICATION_CATEGORY
        End If

        Dim Application As IApplication = Activator.CreateInstance(CachedComponent.Item(AppInfo.Category.ToUpper))
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
        Dim Files As String() = IO.Directory.GetFiles(CUtility.ServerPath(AppInfo.TxnPath), "*.dll")
        For Each File As String In Files
            Try
                Dim FileInfo As New IO.FileInfo(File)
                CUtility.ComponentLoader(FileInfo.DirectoryName, FileInfo.Name.Substring(0, FileInfo.Name.Length - 4), "")
            Catch ex As Exception

            End Try
        Next
        ' TODO The Optimize Later
        For Each Item As Generic.KeyValuePair(Of String, Type) In CUtility.ComponentList
            Try
                CachedComponent.Add(Item.Key, Item.Value)
            Catch ex As Exception

            End Try
        Next

        For Each Item As Generic.KeyValuePair(Of String, Type) In CachedComponent
            Try
                If ContainBaseType(Item.Value, Application.BaseTxnType) Then
                    Application.TxnList.Add(Item.Key, Item.Value)
                Else
                    Application.ComponentList.Add(Item.Key, Item.Value)
                End If
            Catch ex As Exception
            End Try
        Next

        CLog.Info("Transaction Count  : {0}", Application.TxnList.Count)
        CLog.Info("Component Count    : {0}", Application.ComponentList.Count)
        CLog.Info("Transaction Loaded")
        CLog.Sys("Start Application...")
        Try
            Application.Start(AppInfo)
        Catch ex As CError.CBusinessException
            CLog.Err("Application Start Fail, Due to {0}", ex.Message)
            Return ex.ErrCode
        Catch ex As Exception
            CLog.Err(ex.Message)
            Return CError.CErrorCode.INTERNAL_ERROR
        End Try

        Return CError.CErrorCode.SUCCESSFUL
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

End Class

