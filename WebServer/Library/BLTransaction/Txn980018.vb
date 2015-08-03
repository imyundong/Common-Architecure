Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn980018
    Inherits BancslinkTxnBase

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("TellerId", TellerId)
    End Sub
    Property TellerId As String

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)

        Dim Teller As New CTeller
        Try
            Teller.Search(DatabaseFactory, TellerId, False)
            TxnResponse.HostTellerId = Teller.HostTellerId
            TxnResponse.TellerName = Teller.TellerName
            TxnResponse.LeaveDate = Teller.LeaveDate
        Catch ex As Exception
            CLog.Warning(ex.Message)
        End Try
    End Sub

    Private TxnResponse As New TxnResp980018
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property
    Public Class TxnResp980018
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property HostTellerId As Integer = 0
        Property TellerName As String = ""
        Property LeaveDate As String = ""
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("HostTellerId", HostTellerId)
            Message.SetValue("TellerName", TellerName)
            Message.SetValue("LeaveDate", LeaveDate)
            Return Message
        End Function
    End Class

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980018"
        End Get
    End Property
End Class
