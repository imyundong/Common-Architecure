Namespace Data
    Public Class CEnviroment
        Shared Property IsWebApplication As Boolean = False
        Shared Property Status As CSystemStatus = CSystemStatus.Stopped
    End Class

    Public Enum CSystemStatus As Integer
        Stopped = 0
        Started = 2
        Failed = 9
    End Enum
End Namespace