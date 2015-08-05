Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports System.Xml.Serialization
Imports ServerPlatform.Application
''' <summary>
''' Get Sub Teller
''' </summary>
''' <remarks></remarks>
Public Class Txn999989
    Inherits BancslinkTxnBase
    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)

        Dim Teller As New CTeller
        Dim idx As Integer = 0
        While True
            Try
                Teller.SearchByBranch(DatabaseFactory, TellerInfo.BranchId, idx, False)
                Response999989.TellerList.Add(Teller.Clone)
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
            Return 999989
        End Get
    End Property
    Private Response999989 As New CTxnResp999989
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Response999989
        End Get
    End Property


    Public Class CTxnResp999989
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property TellerList As New List(Of CTeller)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TellerList", TellerList)

            Return Message
        End Function
    End Class
End Class
