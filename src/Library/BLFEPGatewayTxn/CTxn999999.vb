Imports ServerPlatform
Imports ServerPlatform.Library.Data

Public Class CTxn999999
    Inherits BLFEPGatewayTxnBase
    Property Data As String
    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("Data", Data)
    End Sub

    Private _Reponse As New TxnResp999999
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Reponse
        End Get
    End Property
    Public Overrides Function ToForeign(HostId As String) As CMessage
        Dim Message As CMessage = MyBase.ToForeign(HostId)
        Message.SetValue("Data", Data)

        Return Message
    End Function
    Public Overrides Function FromForeign(ByVal Idx As Integer, Message As CMessage) As Boolean
        ' Do Check!
        MyBase.FromForeign(Idx, Message)
        Message.GetValueByKey("Data", _Reponse.Data)

        Return False
    End Function

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Todo As String = ""
        HostId = "FEP"
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

    Public Class TxnResp999999
        Inherits BLFEPTxnResponseBase
        Property Data As String
        Public Overrides Function Encode() As CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("Data", Data)

            Return Message
        End Function
    End Class

End Class
