Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
''' <summary>
''' Get Teller List
''' </summary>
''' <remarks></remarks>
Public Class Txn999997
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Teller As New CTeller
        ' Get All Tellers from Database
        Teller.SearchAll(DatabaseFactory, TxnResp999997.TellerList)
    End Sub

    Private TxnResp999997 As New CTxnResp999997
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResp999997
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999997
        End Get
    End Property

    Public Class CTxnResp999997
        Inherits BancslinkTxnBase.CStandardResponseBase
        Public Property TellerList As New List(Of CTeller)
        Public Overrides Function Encode() As ServerPlatform.Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TellerList", TellerList)
            Return Message
        End Function
    End Class
End Class
