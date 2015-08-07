Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Application
Imports ServerPlatform
Imports ServerPlatform.Library.Data

Public MustInherit Class BLFEPGatewayTxnBase
    Inherits CTransactionBase

    Property System As String
    Property HostSerialNo As String

    Public MustOverride Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
    Public MustOverride Overrides ReadOnly Property TxnCode As String

    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("System", System)
        Message.GetValueByKey("HostSerialNo", HostSerialNo)

        Dim Resp As BLFEPTxnResponseBase = DirectCast(Response, BLFEPTxnResponseBase)
        Resp.System = System
        Resp.HostSerialNo = HostSerialNo
        Resp.TxnCode = ClientTxnCode
    End Sub

    Private _Response As New BLFEPTxnResponseBase
    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property

    Public Overrides Function ToForeign(HostId As String) As Library.Data.CMessage
        Dim Message As New CMessage
        Message.TxnCode = ClientTxnCode

        Message.MessageId = HostSerialNo
        Message.SetValue("MessageId", HostSerialNo)
        Message.SetValue("System", System)

        Return Message
    End Function

    Public Class BLFEPTxnResponseBase
        Inherits CTransactionBase.CStandardResponseBase
        Property HostSerialNo As Integer
        Property System As String
        Public Overrides Function Encode() As CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TxnCode", TxnCode)
            Message.SetValue("HostSerialNo", HostSerialNo)
            Message.SetValue("System", System)

            Return Message
        End Function
    End Class
End Class
