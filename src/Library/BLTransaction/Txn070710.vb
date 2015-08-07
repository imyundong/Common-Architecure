Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn070710
    Inherits BancslinkTxnBase

    Property ACCTNO As String    '頁面傳入的值
    Property OLDACCTNO As String


    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim othb As New CCARLOAN
        Try
            If OLDACCTNO IsNot Nothing Then
                If OLDACCTNO.Trim.Length > 0 Then
                    othb.SearchByOLDACCTNO(DatabaseFactory, OLDACCTNO, 0, False)   '帶入老號查新號
                    TxnResponse.ACCTNO = othb.ACCTNO
                End If
            Else
                othb.SearchByACCTNO(DatabaseFactory, ACCTNO, 0, False)   '帶入新號查老號
                TxnResponse.OLDACCTNO = othb.OLDACCTNO
            End If
            

        Catch ex As Exception
            TxnResponse.ACCTNO = ""
            TxnResponse.OLDACCTNO = ""

        End Try
    End Sub


    Private TxnResponse As New TxnResp070710

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property


    'output
    Public Class TxnResp070710
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property ACCTNO As String
        Property OLDACCTNO As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("ACCTNO", ACCTNO)      '將值傳回頁面接收的對應名稱, 傳回DB取回的值
            Message.SetValue("OLDACCTNO", OLDACCTNO)

            Return Message
        End Function
    End Class


    'input
    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("OLDACCTNO", OLDACCTNO)     ' 自設與頁面對應的名稱, 接收頁面值
        Message.GetValueByKey("ACCTNO", ACCTNO)

    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "070710"
        End Get
    End Property
End Class
