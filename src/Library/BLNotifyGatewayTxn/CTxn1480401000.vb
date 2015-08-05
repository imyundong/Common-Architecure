Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports System.Xml.Serialization
Imports ServerPlatform.Application

Public Class Txn1480401000
    Inherits BLNotifyGatewayTxnBase

    Property SenderId As String
    Property BankCd As String
    Property BranchCd As String
    Property SectionCd As String
    Property RoleCd As String
    Property Receiver As String
    Property MessageType As String
    Property Broadcast As String

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("SenderId", SenderId)
        Message.GetValueByKey("BankCd", BankCd)
        Message.GetValueByKey("BranchCd", BranchCd)
        Message.GetValueByKey("SectionCd", SectionCd)
        Message.GetValueByKey("RoleCd", RoleCd)
        Message.GetValueByKey("Receiver", Receiver)
        Message.GetValueByKey("MessageType", MessageType)
        Message.GetValueByKey("Broadcast", Broadcast)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        HostId = "BancslinkAppServer"
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "1480401000"
        End Get
    End Property

    Public Overrides Function ToForeign(HostId As String) As CMessage
        Dim Message As CMessage = MyBase.ToForeign(HostId)
        Message.TxnCode = "970001"

        Message.SetValue("RoleCD", RoleCd)
        Message.SetValue("SenderId", SenderId)
        Message.SetValue("Receiver", Receiver)
        Message.SetValue("MessageType", MessageType)
        Message.SetValue("Broadcast", Broadcast)
        Message.SetValue("SectionCd", SectionCd)
        Message.SetValue("BranchCd", BranchCd)
        Return Message
    End Function
End Class
