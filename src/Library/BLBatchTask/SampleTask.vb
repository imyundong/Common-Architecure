Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility

Public Class SampleTask
    Inherits CTaskBase
    Public Overrides ReadOnly Property Name As String
        Get
            Return "SampleTask"
        End Get
    End Property

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        CLog.Info("This is Sample Task")
        Threading.Thread.Sleep("3000")
    End Sub
End Class
