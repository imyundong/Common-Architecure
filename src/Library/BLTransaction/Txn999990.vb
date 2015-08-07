Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn999990
    Inherits BancslinkTxnBase

    Property Branch As String
    Public Overrides Sub Decode(Message As CMessage) '接收Screen傳過來之資料
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchId", Branch)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        Try
            Dim Cmd As New Text.StringBuilder("SELECT T.TellerId as TellerId, T.TellerName as TellerName")
            Cmd.Append(" FROM Teller as T inner join TellerRole as TR on T.TellerID = TR.TellerId ")
            Cmd.Append(" WHERE TR.TellerRoleId = @TellerRoleId and T.BranchID = @BranchID and T.Enabled = @Enabled")

            Dim Adapter As IDatabaseAdapter = DatabaseFactory.CreateInstance()
            Dim Command As IDbCommand = Adapter.Command
            Command.CommandText = Cmd.ToString
            Adapter.AddWithValue("TellerRoleId", 5)
            Adapter.AddWithValue("BranchID", BranchId)
            Adapter.AddWithValue("Enabled", 1)
            Dim DataAdapter As IDbDataAdapter = Adapter.Adapter
            DataAdapter.SelectCommand = Command

            Dim Ds As New DataSet
            DataAdapter.Fill(Ds)

            For Each Record As DataRow In Ds.Tables(0).Rows
                Dim Teller As New ReturnList
                Teller.TellerId = Record.Item(0)
                Teller.TellerName = Record.Item(1)
                TxnResponse.IBDList.Add(Teller)
            Next
        Catch ex As Exception
            CLog.Err("Retrive Fail {0}", ex.Message)
        End Try
        
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999990
        End Get
    End Property

    Public Class ReturnList
        Implements ICloneable
        Public Property TellerId As Integer
        Public Property TellerName As String

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim Teller As New ReturnList
            Teller.TellerId = TellerId
            Teller.TellerName = TellerName
            Return Teller
        End Function
    End Class

    Public Class TxnResp999990
        Inherits BancslinkTxnBase.CStandardResponseBase

        Property IBDList As New List(Of ReturnList)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("IBDList", IBDList)
            Return Message
        End Function
    End Class

    Private TxnResponse As New TxnResp999990
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return TxnResponse
        End Get
    End Property
End Class
