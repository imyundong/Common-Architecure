Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class CTxn999992
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim System As New CSystemInfo
        System.Search(DatabaseFactory, 1, Nothing)

        _Response.SystemName = System.SystemName
        _Response.SystemVersion = System.Version
    End Sub

    Private _Response As New CTxnResp999992
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999992"
        End Get
    End Property

    Public Class CTxnResp999992
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property SystemName As String
        Property SystemVersion As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("SystemName", SystemName)
            Message.SetValue("SystemVersion", SystemVersion)
            Return Message
        End Function


    End Class
End Class

