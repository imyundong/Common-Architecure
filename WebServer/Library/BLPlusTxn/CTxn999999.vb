Imports System.Xml.Serialization
Imports ServerPlatform.Library.Data

Public Class CTxn999999
    Inherits TransactionBase
    Property CloneMessage As Library.Data.CMessage
    Property HostTxnCode As String
    Property HostTellerNo As String

    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Me.CloneMessage = Message.Clone
        Me.CloneMessage.MessageId = ""
        Me.CloneMessage.OriginalId = ""
    End Sub

    Public Overrides Function ToForeign(HostId As String) As Library.Data.CMessage
        CloneMessage.MessageId = 0
        CloneMessage.SetValue("HostTxnCode", HostTxnCode)
        CloneMessage.SetValue("HostTellerNo", HostTellerNo)
        Return CloneMessage
    End Function

    Private _Response As New TxnResponse999999
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property

    Public Overrides Function FromForeign(Idx As Integer, Message As Library.Data.CMessage) As Boolean
        MyBase.FromForeign(Idx, Message)

        Dim OutputType As String = ""
        Message.GetValueByKey("OutputType", OutputType)

        For i = 0 To Message.Keys.Count - 1
            _Response.ResponseMessage.SetValue(Message.Keys(i), Message.Values(i))
        Next

        ' Check Error Code
        If OutputType = "01" Then
            Message.GetValueByKey("ErrCode", _Response.HostErrCode)
            Message.GetValueByKey("ErrDescription", _Response.HostErrDescription)
            Throw New Library.Workflow.CError.CBusinessException(Library.Workflow.CError.CErrorCode.TXN_FAIL, "Transaction Fail in Host")
        ElseIf OutputType = "08" Then
            _Response.Action = BancslinkTxnResponseBase.CAction.OK
        End If

        ' Bancs Has More Messages

        Dim IsLastMessage As Boolean
        Message.GetValueByKey("LastMessageValue", IsLastMessage)

        Return IsLastMessage
    End Function

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        ' Getting Host
        Dim Host As New CHosts
        Host.Search(DatabaseFactory, Parameter.Host, Nothing)
        HostId = Host.HostName
        HostTellerNo = UserId

        If String.IsNullOrEmpty(Parameter.HostTxnCode) Then
            Throw New ServerPlatform.Library.Workflow.CError.CBusinessException(Library.Workflow.CError.CErrorCode.INVALID_TRANSACTION_CODE, "Host Transaction Code is not Defined")
        End If
        HostTxnCode = Parameter.HostTxnCode
        _Response.SystemId = HostId
    End Sub

    Public Overrides ReadOnly Property IsLocal As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999999"
        End Get
    End Property

    Public Class TxnResponse999999
        Inherits BancslinkTxnResponseBase
        Property ResponseMessage As New CMessage

        Public Overrides Function EncodeError() As CMessage
            Dim Message As CMessage = MyBase.EncodeError()
            Message.SetValue("ErrCode", HostErrCode)
            Message.SetValue("ErrDescription", HostErrDescription)

            Return Message
        End Function
        Public Overrides Function Encode() As CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.Keys = ResponseMessage.Keys
            Message.Values = ResponseMessage.Values

            Return Message
        End Function

    End Class

End Class
