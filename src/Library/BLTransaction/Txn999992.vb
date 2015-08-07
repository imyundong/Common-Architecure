Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class Txn999992
    Inherits BancslinkTxnBase


    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim Branch As New CBranch
        Dim Index As Integer = 0



        Try
            While True
                Branch.SearchByAll(DatabaseFactory, Index, False)
                TxnResponse.BranchList.Add(Branch.Clone)

                Index += 1
            End While
        Catch ex As Library.Workflow.CError.CBusinessException
            If ex.ErrCode <> Library.Workflow.CError.CErrorCode.RECORD_NOT_FOUND Then
                Throw ex
            End If
        Catch ex As Exception
            CLog.Err("Exception {0}", ex.ToString)
        End Try
    End Sub


    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999992
        End Get
    End Property



    Public Class TxnResp999992
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property BranchList As New List(Of CBranch)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("BranchList", BranchList)
            Return Message
        End Function
    End Class

    Private TxnResponse As New TxnResp999992
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property
End Class
