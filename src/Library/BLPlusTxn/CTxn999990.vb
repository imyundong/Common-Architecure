Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

Public Class CTxn999990
    Inherits BancslinkTxnBase
    Property OverrideId As String
    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("OverrideId", OverrideId)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim OverrideHistoryDetail As New CTxnResp999990.COverrideHistoryDetail
        Try
            Dim Idx As Integer = 0
            While (True)
                OverrideHistoryDetail.SearchByOverrideId(DatabaseFactory, OverrideId, Idx, Nothing)
                Dim Copy As CTxnResp999990.COverrideHistoryDetail = OverrideHistoryDetail.Clone

                If BancslinkTxnBase.OverrideCode.ContainsKey(OverrideHistoryDetail.OverrideCode) Then
                    Dim OverrideCode As COverrideCode = BancslinkTxnBase.OverrideCode.Item(OverrideHistoryDetail.OverrideCode)
                    Copy.OverrideDescription = OverrideCode.OverrideDescription
                    Copy.Capability = OverrideCode.Capability
                End If

                If BancslinkTxnBase.OverrideStatus.ContainsKey(Copy.Status) Then
                    Copy.StatusDescription = BancslinkTxnBase.OverrideStatus.Item(Copy.Status).Description
                End If

                Dim User As New CSystemUser
                User.Search(DatabaseFactory, Copy.UserId, Nothing)
                Copy.Username = User.FullName

                User.Search(DatabaseFactory, Copy.SupervisorId, Nothing)
                Copy.SupervisorName = User.FullName

                Idx += 1
                _Response.OverrideHistoryList.Add(Copy)
            End While

            Dim OverrideHistory As New CTxnResp999990.COverrideHistoryDetail
        Catch ex As Exception

        End Try
    End Sub

    Private _Response As New CTxnResp999990
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999990"
        End Get
    End Property

    Public Class CTxnResp999990
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        <Serializable>
        Public Class COverrideHistoryDetail
            Inherits COverrideHistory

            Property OverrideDescription As String = ""
            Property Username As String
            Property SupervisorName As String
            Property StatusDescription As String

            Property Capability As Integer

            Public Overrides Function Clone() As Object
                Dim OverrideHistory As New COverrideHistoryDetail
                OverrideHistory.SequenceNo = SequenceNo
                OverrideHistory.OverrideId = OverrideId
                OverrideHistory.OverrideCode = OverrideCode
                OverrideHistory.UserId = UserId
                OverrideHistory.SupervisorId = SupervisorId
                OverrideHistory.Status = Status
                OverrideHistory.RequestDate = RequestDate
                OverrideHistory.UpdateDate = UpdateDate
                OverrideHistory.OverrideDescription = OverrideDescription
                OverrideHistory.Capability = Capability
                OverrideHistory.SupervisorName = SupervisorName
                OverrideHistory.Username = Username
                OverrideHistory.StatusDescription = StatusDescription

                Return OverrideHistory
            End Function
        End Class

        Property OverrideHistoryList As New List(Of COverrideHistoryDetail)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()

            Message.SetValue("OverrideHistoryList", OverrideHistoryList)
            Return Message
        End Function
    End Class
End Class

