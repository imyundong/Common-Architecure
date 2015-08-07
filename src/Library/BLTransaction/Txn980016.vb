Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn980016
    Inherits BancslinkTxnBase

    Property PayrollCode As String

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim CompanyBusiness As New CCompanyBusiness

        Try
            CompanyBusiness.Search(DatabaseFactory, PayrollCode, Nothing)
            TxnResponse.CompanyName = CompanyBusiness.CompanyName
            TxnResponse.CompanyID = CompanyBusiness.CompanyId
        Catch ex As Exception
            TxnResponse.CompanyName = " "
        End Try
    End Sub

    Private TxnResponse As New TxnResp980016
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property
    Public Class TxnResp980016
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property CompanyName As String
        Property CompanyID As String
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("CompanyName", CompanyName)
            Message.SetValue("CompanyID", CompanyID)
            Return Message
        End Function
    End Class

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("PayrollCode", PayrollCode)

    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980016"
        End Get
    End Property
End Class
