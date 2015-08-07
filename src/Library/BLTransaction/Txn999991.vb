Imports ServerPlatform.Library.Data
''' <summary>
''' Get Sub Teller
''' </summary>
''' <remarks></remarks>
Public Class Txn999991
    Inherits BancslinkTxnBase
    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)


        'Dim Idx As Integer = 0
        'While True
        'Teller.SearchBySameBranchEnabledCpaOver5(DatabaseFactory, 1, TellerInfo.BranchId, 5, Idx, False)
        'Response999991.TellerList.Add(Teller.Clone)
        'Idx += 1
        'End While



        'Dim Idx As Integer = 0
        'While True
        ' Teller.Search(DatabaseFactory, TellerInfo.BranchId, False)
        ' Response999991.TellerList.Add(Teller.Clone)
        ' Exit While
        ' End While



        Dim idx As Integer = 0
        While True
            Try
                Dim Teller As New CTeller
                ''Teller.SearchByEnabled(DatabaseFactory, 1, idx, False)
                Teller.SearchBySameBranchEnabledCpaOver5(DatabaseFactory, 1, TellerInfo.BranchId, 5, idx, False)
                Response999991.TellerList.Add(Teller.Clone)
                idx += 1
            Catch ex As Exception
                Exit While
            End Try
        End While



        'Teller.SearchAll(DatabaseFactory, Response999991.TellerList)
    End Sub

    Private Response999991 As New CTxnResp999991
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Response999991
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999991
        End Get
    End Property

    Public Class CTxnResp999991
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property TellerList As New List(Of CTeller)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("TellerList", TellerList)

            Return Message
        End Function
    End Class
End Class
