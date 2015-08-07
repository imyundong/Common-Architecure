Imports ServerPlatform.Library.Data
Public Class Txn999995
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)

        Dim idx As Integer = 0
        While True
            Try
                Dim Teller As New CTeller
                Teller.SearchByEnabled(DatabaseFactory, 1, idx, False)

                TxnResp999995.OnlineTellerList.Add(Teller.Clone)
                idx += 1
            Catch ex As Exception
                Exit While
            End Try
        End While

        'Teller.SearchAll(DatabaseFactory, TxnResp999995.OnlineTellerList)
    End Sub

    Private TxnResp999995 As New CTxnResp999995
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResp999995
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999995
        End Get
    End Property

    Class CTxnResp999995
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property OnlineTellerList As New List(Of CTeller)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("OnlineTellerList", OnlineTellerList)
            Return Message
        End Function
    End Class
End Class
