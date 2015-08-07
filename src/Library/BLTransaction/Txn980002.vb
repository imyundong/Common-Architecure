Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn980002
    Inherits BancslinkTxnBase

    Property RegisterPhoneNoA As String
    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("RegisterPhoneNoA", RegisterPhoneNoA)
    End Sub


    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        'Threading.Thread.Sleep(1000)
        Dim CCombolist As New CComboboxList
        Try
            CCombolist.SearchByTELECODE(DatabaseFactory, "TELECODE", RegisterPhoneNoA, 0, False)
            TxnResponse.TestValue = RegisterPhoneNoA
        Catch ex As Exception

            Try
                CCombolist.SearchByTELECODE(DatabaseFactory, "TELECODE", Strings.Left(RegisterPhoneNoA, 3), 0, False)
                TxnResponse.TestValue = Strings.Left(RegisterPhoneNoA, 3)
            Catch ex1 As Exception
                Try
                    CCombolist.SearchByTELECODE(DatabaseFactory, "TELECODE", Strings.Left(RegisterPhoneNoA, 2), 0, False)
                    TxnResponse.TestValue = Strings.Left(RegisterPhoneNoA, 2)
                Catch ex2 As Exception
                    TxnResponse.TestValue = ""
                End Try
            End Try
        End Try

    End Sub

    Private TxnResponse As New TxnResp980002

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "980002"
        End Get
    End Property

    Public Class TxnResp980002
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property TestValue As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TestValue", TestValue)

            Return Message
        End Function
    End Class
End Class
