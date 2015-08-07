Imports ServerPlatform.Library.Data
''' <summary>
''' Get Sub Branch
''' </summary>
''' <remarks></remarks>
Public Class Txn999996
    Inherits BancslinkTxnBase
    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Branch As New CBranch
        ' Dim Idx As Integer = 0
        'While True
        '    Branch.SearchByParent(DatabaseFactory, TellerInfo.BranchID, Idx, False)
        '    Idx += 1
        'End While
        Branch.SearchAll(DatabaseFactory, Response999996.BranchList)
    End Sub

    Private Response999996 As New CTxnResp999996
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Response999996
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999996
        End Get
    End Property

    Public Class CTxnResp999996
        Inherits BancslinkTxnBase.CStandardResponseBase
        Property BranchList As New List(Of CBranch)
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("BranchList", BranchList)

            Return Message
        End Function
    End Class
End Class
