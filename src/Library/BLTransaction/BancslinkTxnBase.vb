Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Application
Public MustInherit Class BancslinkTxnBase
    Inherits CTransactionBase
    Property TellerInfo As CTeller
    Property BranchId As Integer

    Public MustOverride Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
    Public MustOverride Overrides ReadOnly Property TxnCode As String

    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchId", BranchId)
    End Sub

    Public Overrides Sub PreProcess(DatabaseFactory As CDatabaseFactory)
        CLog.Info("User Id {0}, Branch {0}", UserId, BranchId)
        TellerInfo = New CTeller
        TellerInfo.Search(DatabaseFactory, UserId, False)

        CLog.Info("Teller Branch    : {0}", TellerInfo.BranchID)
        Dim Terminal As String = TellerInfo.TerminalID
        If String.IsNullOrEmpty(Terminal) Then Terminal = 0
        CLog.Info("Current Termianl : {0}", Terminal)
    End Sub

    Public Class BancslinkTxnResponseBase
        Inherits CTransactionBase.CStandardResponseBase

        Property Action As CAction = CAction.[Default]
        Public Enum CAction As Integer
            [Default] = 0
            [Error] = 4
            OK = 8
            SUP = 1
            NEWSCREEN = 3
        End Enum

        Public Overrides Function EncodeError() As Library.Data.CMessage
            Dim Message As Library.Data.CMessage = MyBase.EncodeError()
            Message.SetValue("Action", CInt(Action))

            Return Message
        End Function

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As Library.Data.CMessage = MyBase.Encode()
            Message.SetValue("Action", CInt(Action))

            Return Message
        End Function
    End Class
End Class
