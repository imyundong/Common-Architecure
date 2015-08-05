Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn980005
    Inherits BancslinkTxnBase
    Property TellerID As String    

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim Teller As New CTeller
        Try

            Teller.Search(DatabaseFactory, TellerID, False)
            TxnResponse.TellerName = Teller.TellerName

        Catch ex As Exception
            TxnResponse.TellerName = ""

        End Try
    End Sub

    Private TxnResponse As New TxnResp980005
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980005
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property TellerName As String


        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TellerName", TellerName)
            Return Message
        End Function
    End Class

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("TellerID", TellerID)        
    End Sub
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980005"
        End Get
    End Property



End Class
