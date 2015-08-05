Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports System.Xml.Serialization
Imports ServerPlatform.Application
Public Class Txn999988
    Inherits BancslinkTxnBase

    Property CompabyId As String '統一編號
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("CompabyId", CompabyId)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim Company As New CCompanyBusiness
        Dim idx As Integer = 0
        While True
            Try
                Company.SearchByPayrollCode(DatabaseFactory, CompabyId, idx, False)
                Response999988.CompanyList.Add(Company.Clone)
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
            Return 999988
        End Get
    End Property

    Private Response999988 As New CTxnResp999988
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Response999988
        End Get
    End Property

    Public Class CTxnResp999988
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property CompanyList As New List(Of CCompanyBusiness)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("CompanyList", CompanyList)

            Return Message
        End Function
    End Class
End Class
