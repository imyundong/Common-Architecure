Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn980015
    Inherits BancslinkTxnBase

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim cmskey As New CCMSKEY
        Try

            cmskey.Search(DatabaseFactory, "MKEY", False)
            TxnResponse.MKEY = cmskey.KEYVALUE
            cmskey.Search(DatabaseFactory, "SKEY", False)
            TxnResponse.SKEY = cmskey.KEYVALUE
        Catch ex As Exception
            TxnResponse.MKEY = ""
            TxnResponse.SKEY = ""
        End Try
    End Sub

    Private TxnResponse As New TxnResp980015
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Class TxnResp980015
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property MKEY As String
        Property SKEY As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("MKEY", MKEY)
            Message.SetValue("SKEY", SKEY)
            Return Message
        End Function
    End Class


    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980015"
        End Get
    End Property
End Class
