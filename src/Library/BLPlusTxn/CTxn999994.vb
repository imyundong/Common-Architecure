Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class CTxn999994
    Inherits TransactionBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)

        Try
            Dim Idx As Integer = 0
            ' Get All Plugins
            Dim PluginList As New List(Of CPluginList)
            CPluginList.SearchAll(DatabaseFactory, PluginList)

            Dim UserPlugin As New List(Of Integer)
            Try
                Dim Plugin As New CUserPlugin
                While (True)
                    Plugin.SearchByUserId(DatabaseFactory, UserId, Idx, Nothing)
                    UserPlugin.Add(Plugin.PluginId)
                    Idx += 1
                End While
            Catch ex As Exception

            End Try

            CLog.Info("{0}/{1} Plugin Found For Current User {2}", UserPlugin.Count, PluginList.Count, UserId)
            For Each Item As CPluginList In PluginList
                If UserPlugin.Contains(Item.PluginId) Then
                    Item.Available = True
                End If

                _Response.PluginList.Add(Item)
            Next

        Catch ex As Exception

        End Try
    End Sub

    Private _Response As New CTxnResp999994
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999994"
        End Get
    End Property

    Public Class CTxnResp999994
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property PluginList As New List(Of CPluginList)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("PluginList", PluginList)

            Return Message
        End Function


    End Class
End Class

