Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data

Public MustInherit Class CTransactionBase
    Implements ITransaction
    Implements IComponent
    Overridable Sub PreProcess(DatabaseFactory As CDatabaseFactory)
        ' TODO
    End Sub
    Overridable Sub PostProcess(ErrCode As Library.Workflow.CError.CErrorCode, ErrDescription As String, ProcTime As Long)
        ' TODO
    End Sub
    Public Const PRE_EXECUTED_TXN As String = "PRE_EXECUTED_TXN"
    MustOverride Sub Process(DatabaseFactory As CDatabaseFactory)
    Property ClientTxnCode As String
    Property TxnDate As Date
    Property SysDate As Date
    Property UserId As String
    Property HostId As String
    Overridable ReadOnly Property IsLocal As Boolean
        Get
            Return True
        End Get
    End Property

    Overridable Sub Decode(ByVal Message As CMessage)
        SysDate = Message.SystemDate
        TxnDate = Message.TxnDate
        UserId = Message.UserId
    End Sub

    Overridable Function ToForeign(ByVal HostId As String) As CMessage
        Dim Message As New CMessage
        Message.TxnCode = TxnCode
        Message.SystemDate = Now

        Return Message
    End Function

    Overridable Function FromForeign(ByVal Idx As Integer, ByVal Message As CMessage) As Boolean
        ' TODO
        Return False
    End Function

    Public Overridable Sub Debug() Implements ITransaction.Debug
        CLog.Debug("Txn Code", TxnCode)
        CLog.Debug("System Date", SysDate)
        CLog.Debug("Txn Date", TxnDate)
        CLog.Debug("User Id", UserId)
    End Sub

    Public MustOverride ReadOnly Property TxnCode As String Implements ITransaction.TxnCode

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return TxnCode.ToString
        End Get
    End Property

    Private _Response As New CStandardResponseBase
    Overridable ReadOnly Property Response As CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property

    Public Class CStandardResponseBase
        Property TxnCode As Integer = 999999
        Property SystemId As String = "Bancslink Server"
        Property ErrCode As ServerPlatform.Library.Workflow.CError.CErrorCode = Library.Workflow.CError.CErrorCode.SUCCESSFUL

        Overridable Function Encode() As CMessage
            Dim Message As New CMessage
            Message.TxnCode = TxnCode
            Message.SystemDate = Now
            Message.ErrCode = ErrCode
            Message.SystemId = SystemId
            Return Message
        End Function

        Overridable Function EncodeError() As CMessage
            Dim Message As New CMessage
            Message.TxnCode = 0
            Message.SystemDate = Now
            Message.ErrCode = ErrCode
            Message.SystemId = SystemId

            Return Message
        End Function
    End Class
End Class
