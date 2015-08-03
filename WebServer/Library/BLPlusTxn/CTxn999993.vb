Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class CTxn999993
    Inherits TransactionBase
    Property ToUse As Boolean
    Property PluginId As Integer

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("PluginId", PluginId)
        Message.GetValueByKey("ToUse", ToUse)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim UserPlugin As New CUserPlugin
        UserPlugin.PluginId = PluginId
        UserPlugin.UserId = UserId

        If ToUse = False Then
            Try
                UserPlugin.Remove(DatabaseFactory)
            Catch ex As Exception

            End Try
        Else
            Try
                UserPlugin.Insert(DatabaseFactory)
            Catch ex As Exception
            End Try
        End If

        Try
            UserPlugin.Search(DatabaseFactory, UserId, PluginId, Nothing)
            _Response.InUse = True
        Catch e As Exception
            _Response.InUse = False
        End Try
    End Sub

    Private _Response As New CTxnResp999993
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999993"
        End Get
    End Property

    Public Class CTxnResp999993
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property InUse As Boolean

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("InUse", InUse)

            Return Message
        End Function


    End Class
End Class

