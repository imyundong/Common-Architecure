Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn999993
    Inherits BancslinkTxnBase
    Property SelectOption As String = ""

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("Option", SelectOption)
    End Sub

    Public Overrides Sub Debug()
        MyBase.Debug()
        CLog.Debug("Option", SelectOption)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Dim Index As Integer = 0
        Dim TxnPath As New CTxnPath
        Dim Parent As Integer = 0
        Try
            Parent = CInt(SelectOption.Split("_"c)(1))
            CLog.Info("Parent Is {0}", Parent)
        Catch ex As Exception

        End Try

        Try
            While True
                TxnPath.SearchByParent(DatabaseFactory, Parent, Index, False)
                TxnResponse.TxnPathList.Add(TxnPath.Clone)

                Index += 1
            End While
        Catch ex As Exception

        End Try
    End Sub

    Private TxnResponse As New TxnResp999993
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999993
        End Get
    End Property

    Public Class TxnResp999993
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property TxnPathList As New List(Of CTxnPath)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TxnPathList", TxnPathList)
            Return Message
        End Function
    End Class
End Class
