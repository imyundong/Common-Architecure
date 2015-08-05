Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Public Class Txn970001
    Inherits BancslinkTxnBase

    Property SenderId As String
    Property BankCd As String
    Property BranchCd As String
    Property RoleCD As String
    Property Receiver As String
    Property MessageType As String
    Property Broadcast As String
    Property SectionCD As String

    Public Overrides Sub PreProcess(DatabaseFactory As CDatabaseFactory)
        ' TODO
    End Sub

    Public Overrides Sub Decode(Message As CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("SenderId", SenderId)
        Message.GetValueByKey("BankCd", BankCd)
        Message.GetValueByKey("BranchCd", BranchCd)
        Message.GetValueByKey("RoleCD", RoleCd)
        Message.GetValueByKey("Receiver", Receiver)
        Message.GetValueByKey("MessageType", MessageType)
        Message.GetValueByKey("Broadcast", Broadcast)
        Message.GetValueByKey("SectionCD", SectionCD)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)

        Try
            Dim Command As SqlClient.SqlCommand = DatabaseFactory.CreateInstance.Command()

            Command.CommandType = CommandType.StoredProcedure
            Command.CommandText = "Messenger_Add"

            Command.Parameters.AddWithValue("@SenderId", SenderId)
            Command.Parameters.AddWithValue("@BranchCd", BranchCd)
            Command.Parameters.AddWithValue("@SectionCd", SectionCD)
            Command.Parameters.AddWithValue("@RoleCd", RoleCD)
            Command.Parameters.AddWithValue("@Receiver", Receiver)
            Command.Parameters.AddWithValue("@MessageType", MessageType)
            Command.Parameters.AddWithValue("@Broadcast", Broadcast)

            Command.ExecuteNonQuery()
        Catch ex As Exception
            CLog.Warning("Messenger_Add Error : {0}", ex.Message)
        End Try
    End Sub

    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "970001"
        End Get
    End Property
End Class
