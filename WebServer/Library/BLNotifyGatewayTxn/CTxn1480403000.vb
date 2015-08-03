Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports System.Xml.Serialization
Imports ServerPlatform.Application

Public Class Txn1480403000
    Inherits BLNotifyGatewayTxnBase

    Property BRANCHCD As String
    Property ISCENTER As String
    Property RESULT As String

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("BRANCHCD", BRANCHCD)
        Message.GetValueByKey("ISCENTER", ISCENTER)
        Message.GetValueByKey("RESULT", RESULT)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        HostId = "BancslinkAppServer"
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "1480403000"
        End Get
    End Property

    Public Overrides Function ToForeign(HostId As String) As CMessage
        Dim Message As CMessage = MyBase.ToForeign(HostId)
        Message.TxnCode = "970002"

        Message.SetValue("BRANCHCD", BRANCHCD)
        Message.SetValue("ISCENTER", ISCENTER)
        Return Message
    End Function
End Class
