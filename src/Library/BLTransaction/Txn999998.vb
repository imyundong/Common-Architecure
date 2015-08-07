Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports System.Xml.Serialization
Imports ServerPlatform.Application

Public Class Txn999998
    Inherits BancslinkTxnBase
    Property ComboOption As String
    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("Option", ComboOption)
    End Sub

    Public Overrides Sub Debug()
        MyBase.Debug()
        CLog.Debug("Option", ComboOption)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        CLog.Info("Txn 999998")
        CLog.Info("Get  Options")

        Dim Combo As New CComboboxList
        Dim idx As Integer = 0
        While True
            Try
                Combo.SearchByComboId(DatabaseFactory, ComboOption, idx, False)

                TxnResp999998.SelectOptions.Add(Combo.Clone)
                idx += 1
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
            Return 999998
        End Get
    End Property

    Private TxnResp999998 As New CTxnResp999998
    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return TxnResp999998
        End Get
    End Property

    Public Class CTxnResp999998
        Inherits CStandardResponseBase

        Property SelectOptions As New List(Of CComboboxList)
        Public Overrides Function Encode() As ServerPlatform.Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("SelectOptions", SelectOptions)

            Return Message
        End Function
    End Class

End Class
