Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class CTxn999991
    Inherits BancslinkTxnBase
    Property JournalId As Integer
    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("JournalId", JournalId)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Journal As New CJournal
        Try
            Journal.Search(DatabaseFactory, JournalId, Nothing)
            _Response.PageData = Journal.PageData

            Dim Serializer As New Xml.Serialization.XmlSerializer(GetType(CMessage))
            Using Ms As New IO.MemoryStream(Text.UTF8Encoding.UTF8.GetBytes(Journal.Request))
                Dim Message As CMessage = Serializer.Deserialize(Ms)
                _Response.Message.Keys = Message.Keys
                _Response.Message.Values = Message.Values
            End Using

        Catch ex As Exception

        End Try
    End Sub

    Private _Response As New CTxnResp999991
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999991"
        End Get
    End Property

    Public Class CTxnResp999991
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property PageData As String
        Property Message As New CMessage


        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.Keys = Me.Message.Keys
            Message.Values = Me.Message.Values
            Message.TableKeys = Me.Message.TableKeys
            Message.TableValues = Me.Message.TableValues

            Message.SetValue("PageData", PageData)
            Return Message
        End Function
    End Class
End Class

