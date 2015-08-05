Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn970002
    Inherits BancslinkTxnBase

    Property BRANCHCD As String
    Property ISCENTER As String
    Property RESULT As String

    Public Overrides Sub PreProcess(DatabaseFactory As CDatabaseFactory)
        ' TODO
    End Sub

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("BRANCHCD", BRANCHCD)
        Message.GetValueByKey("ISCENTER", ISCENTER)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        'O -開啟匯款集中作業 --> 1-集中模式
        'C -關閉匯款集中作業 --> 2-分行模式
        If ISCENTER = "O" Then ISCENTER = "1" Else ISCENTER = "2"

        Try
            Dim Command As SqlClient.SqlCommand = DatabaseFactory.CreateInstance.Command()

            Command.CommandType = CommandType.Text
            Command.CommandText = "Update BranchST Set REMITTANCESTATUS = @REMITTANCESTATUS WHERE BANKCD = '806' And BRANCHCD = @BRANCHCD"

            Command.Parameters.AddWithValue("@REMITTANCESTATUS", ISCENTER)
            Command.Parameters.AddWithValue("@BRANCHCD", BRANCHCD)

            Command.ExecuteNonQuery()
        Catch ex As Exception
            CLog.Warning("Update BranchST REMITTANCSTATUS Error : {0}", ex.Message)
        End Try
    End Sub

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "970002"
        End Get
    End Property
End Class
