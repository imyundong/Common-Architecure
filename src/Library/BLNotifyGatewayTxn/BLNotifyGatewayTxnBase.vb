Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Application
Public MustInherit Class BLNotifyGatewayTxnBase
    Inherits CTransactionBase

    Public Class BLNotifyGatewayResponseBase
        Inherits CTransactionBase.CStandardResponseBase

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message = MyBase.Encode()

            Return Message
        End Function


        Public Overrides Function EncodeError() As Library.Data.CMessage
            ' Dim Message = MyBase.EncodeError()
            Dim Message = Encode()
            Message.SetValue("RtCode", CInt(ErrCode))

            Return Message
        End Function
    End Class

    Property ClientAppSeq As String
    Property ClientAppGrpSeq As String
    Property ServerAppSeq As String
    Property RefTimeout As String
    Property ReqUserId As String
    Property ClientGrp As String
    Property AppName As String
    Property Password As String

    Private _Response As New BLNotifyGatewayResponseBase
    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property

    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)

        Message.GetValueByKey("ClientAppSeq", ClientAppSeq)
        Message.GetValueByKey("ClientAppGrpSeq", ClientAppGrpSeq)
        Message.GetValueByKey("ServerAppSeq", ServerAppSeq)
        Message.GetValueByKey("RefTimeout", RefTimeout)
        Message.GetValueByKey("ClientGrp", ClientGrp)
        Message.GetValueByKey("AppName", AppName)
        Message.GetValueByKey("Password", Password)
        _Response.TxnCode = TxnCode

    End Sub

    Public Overrides Function ToForeign(HostId As String) As Library.Data.CMessage
        Dim Message As Library.Data.CMessage = MyBase.ToForeign(HostId)
        Message.IsAdviced = True
        Message.TxnDate = Now()
        Message.SystemId = "BLNotifyGateway"

        Return Message
    End Function

    Public Overrides ReadOnly Property IsLocal As Boolean
        Get
            Return False
        End Get
    End Property
End Class
