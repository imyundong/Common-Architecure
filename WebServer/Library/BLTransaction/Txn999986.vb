Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow

Public Class Txn999986
    Inherits BancslinkTxnBase

    Property TellerId As String
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("TellerId", TellerId)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Role As New CTellerRole
        Dim idx As Integer = 0
        While True
            Try

                Role.SearchByTellerId(DatabaseFactory, TellerId, idx, False)

                Response999986.RoleList.Add(Role.Clone)
                idx += 1
            Catch ex As Exception
                Exit While
            End Try
        End While
    End Sub

    Private Response999986 As New CTxnResp999986
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Response999986
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999986
        End Get
    End Property

    Public Class CTxnResp999986
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property RoleList As New List(Of CTellerRole)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("RoleList", RoleList)

            Return Message
        End Function
    End Class
End Class
