Imports ServerPlatform.Library.Data

Namespace Utility
    Public Interface IApplication
        Sub Start(ByRef AppInfo As CAppConfig.CApplicationInfo)

        Sub [Stop]()

        ReadOnly Property AppConfig As CApplicationConfig
        ' Validate During Txn Loading
        ReadOnly Property BaseTxnType As Type
        Property AppStatus As CAppStatus
    End Interface
End Namespace

