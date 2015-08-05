Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class Txn999987
    Inherits BancslinkTxnBase

    Property QueryString As String

    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("queryStrKey", QueryString)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)

        Dim Idx As Integer = 0
        While True
            Try
                Dim Combo As New CComboboxList
                Combo.SearchLikeComboId(DatabaseFactory, QueryString, Idx, Nothing)
                TxnResponse.ComboList.Add(Combo.Clone)
                Idx += 1
            Catch ex As CError.CBusinessException
                If ex.ErrCode <> CError.CErrorCode.RECORD_NOT_FOUND Then
                    CLog.Err("Retrive Fail {0}: {1}", ex.ErrCode, ex.Message)
                End If
                Exit While
            Catch ex As Exception
                CLog.Err("Retrive Fail {0}", ex.Message)
                Exit While
            End Try

        End While

    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999987
        End Get
    End Property

    Public Class TxnResp999987
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property ComboList As New List(Of CComboboxList)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("ComboList", ComboList)
            Return Message
        End Function
    End Class

    Private TxnResponse As New TxnResp999987

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property
End Class
