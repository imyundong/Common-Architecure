Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility

Public Class UnitTest_Txn999997
    Inherits CTaskBase
    Public Overrides ReadOnly Property Name As String
        Get
            Return "UnitTest_Txn999997"
        End Get
    End Property

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        CLog.Info("Testing Transaction 999997")

        Dim Message As New ServerPlatform.Library.Data.CMessage
        Message.TxnCode = "999997"
        Message.TxnDate = Now()
        Message.UserId = "30034"
        Message.SystemDate = Now

        'Dim MQ As New Messaging.MessageQueue(".\Private$\RequestQ")
        'MQ.Formatter = New Messaging.XmlMessageFormatter({GetType(ServerPlatform.Library.Data.CMessage)})
        'MQ.Send(New Messaging.Message(Message))
    End Sub
End Class
