Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980000
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Threading.Thread.Sleep(1000)
        TxnResponse.TestValue = "OK"
    End Sub


    Private TxnResponse As New TxnResp980000

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980000"
        End Get
    End Property

    Public Class TxnResp980000
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property TestValue As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TestValue", TestValue)

            Return Message
        End Function
    End Class
End Class
