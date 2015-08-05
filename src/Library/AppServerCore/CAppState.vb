Imports ServerPlatform.Library.Data

Public Class CAppState
    Property AppStatus As CAppStatus
    Property Domain As AppDomain
    Property AppLoader As CAppLoader

    Sub New(AppStatus As CAppStatus, Domain As AppDomain)
        Me.AppStatus = AppStatus
        Me.Domain = Domain
    End Sub
End Class