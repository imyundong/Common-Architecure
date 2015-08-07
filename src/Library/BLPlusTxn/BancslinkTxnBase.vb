Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Application
Imports ServerPlatform.Library.Data
Imports System.Threading.Tasks

Public MustInherit Class BancslinkTxnBase
    Inherits CTransactionBase

    ' Keep User Roles
    Shared Property UserRoles As New Generic.Dictionary(Of Integer, CUserRoleCategory)
    ' Keep Txn Parameters
    Shared Property TxnParameters As New Generic.Dictionary(Of String, CTxnParameter)
    ' Keep Override List
    Shared Property OverrideList As New List(Of COverrideList)
    ' Keep Override Code
    Shared Property OverrideCode As New Generic.Dictionary(Of String, COverrideCode)
    ' Keep Override Status
    Shared Property OverrideStatus As New Generic.Dictionary(Of String, COverrideStatus)

    Shared Function GetUserRoles(ByVal DatabaseFactory As CDatabaseFactory, ByVal TellerId As String) As List(Of CUserRoleCategory)
        Dim UserRole As New CUserRole
        Dim Roles As New List(Of CUserRoleCategory)

        Dim Idx As Integer
        Try
            While (True)
                UserRole.SearchByUserId(DatabaseFactory, TellerId, Idx, Nothing)
                Idx += 1

                If UserRoles.ContainsKey(UserRole.RoleId) Then
                    Roles.Add(UserRoles.Item(UserRole.RoleId))
                End If
            End While
        Catch ex As Exception

        End Try

        Return Roles
    End Function
    Public Function GetSystemParameter() As CTxnParameter
        If TxnParameters.ContainsKey(ClientTxnCode) Then
            Return TxnParameters.Item(ClientTxnCode)
        Else
            Throw New CError.CBusinessException(CError.CErrorCode.TXN_PARAMETER_NOT_DEFINED, "Txn Parameter {0} For Page {1} Is Not Defined", ClientTxnCode, PageId)
        End If
    End Function

    Protected Parameter As CTxnParameter
    Property IPAddress As String
    Property Branch As Integer
    Property Terminal As Integer
    Property PageId As String
    Property SupervisorId As Integer
    Property SupervisorToken As String
    Property WorkflowId As String
    Public MustOverride Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
    Public MustOverride Overrides ReadOnly Property TxnCode As String
    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("BranchNo", Branch)
        Message.GetValueByKey("Terminal", Terminal)
        Message.GetValueByKey("PageId", PageId)
        Message.GetValueByKey("SupervisorId", SupervisorId)
        Message.GetValueByKey("SupervisorToken", SupervisorToken)
        Message.GetValueByKey("WorkflowId", WorkflowId)

        Me.Message = Message
        IPAddress = Message.IPAddress
        HostId = "Bancslink Server"
    End Sub

    Protected Message As CMessage
    Private _Response As New BancslinkTxnResponseBase
    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property

    Property UserInfo As CSystemUser
    Property UserBranch As CBranch
    Property Journal As CJournal
    Public Overrides Sub PreProcess(DatabaseFactory As CDatabaseFactory)
        ' Get System Parameters
        Parameter = GetSystemParameter()
        Dim AppConfig As CStandardAppConfig = CStandardAppConfig.GetInstance()

        ' Log Journal
        Journal = New CJournal
        Journal.Status = CJournal.CJournalStatus.Unknow

        Message.GetValueByKey("Account", Journal.Account)
        Message.GetValueByKey("Amount", Journal.Amount)
        Message.GetValueByKey("Currency", Journal.Currency)
        Message.GetValueByKey("PageData", Journal.PageData)
        Message.GetValueByKey("OverrideId", Journal.OverrideId)

        Journal.HostId = Parameter.Host

        Dim Serializer As New Xml.Serialization.XmlSerializer(GetType(CMessage))
        Using Ms As New IO.MemoryStream
            Serializer.Serialize(Ms, Message)
            Ms.Position = 0
            Using Sr As New IO.StreamReader(Ms)
                Journal.Request = Sr.ReadToEnd()
            End Using
        End Using
        Journal.TxnCode = ClientTxnCode
        Integer.TryParse(UserId, Journal.UserId)
        Journal.Supervisor = SupervisorId
        Journal.Terminal = Terminal
        Journal.GroupId = WorkflowId
        Journal.BranchId = Branch

        Dim DBFactory As New CDatabaseFactory(AppConfig.Database())
        Try
            CLog.Info("Insert Journal")
            Journal.Insert(DBFactory)
            CLog.Info("Commit : {0}", Journal.JournalId)
            DBFactory.DatabaseAdapter.Commit()
            CLog.Info("Commit OK")
        Catch ex As Exception
            CLog.Warning("Insert Journal Fail {0}", ex.Message)
            DBFactory.DatabaseAdapter.Rollback()
        Finally
            DBFactory.DatabaseAdapter.Close()
        End Try

    End Sub
    Public Overrides Sub PostProcess(ErrCode As CError.CErrorCode, ErrDescription As String, ProcTime As Long)
        MyBase.PostProcess(ErrCode, ErrDescription, ProcTime)
        Dim Response As BancslinkTxnResponseBase = DirectCast(Me.Response, BancslinkTxnResponseBase)
        ' TODO
        ' Update Journal
        If Journal IsNot Nothing AndAlso Journal.JournalId > 0 Then
            Dim RespMessage As CMessage
            CLog.Info("Update Journal : {0}", Journal.JournalId)
            If ErrCode <> CError.CErrorCode.SUCCESSFUL Then
                Journal.ErrCode = ErrCode

                If Not String.IsNullOrEmpty(Response.HostErrCode) Then
                    Journal.ErrCode = Response.HostErrCode
                End If

                Journal.ErrDescription = ErrDescription
                If Not String.IsNullOrEmpty(Response.HostErrDescription) Then
                    Journal.ErrDescription = Response.HostErrDescription
                End If

                Response.ErrCode = ErrCode

                Journal.Status = CJournal.CJournalStatus.Fail
                RespMessage = Response.EncodeError
            Else
                Journal.ErrCode = CInt(ErrCode)
                Journal.ErrDescription = ErrCode.ToString
                Journal.Status = CJournal.CJournalStatus.Successful
                RespMessage = Response.Encode
            End If
            Journal.ProcTime = ProcTime

            Dim Serializer As New Xml.Serialization.XmlSerializer(GetType(CMessage))
            Using Ms As New IO.MemoryStream
                Serializer.Serialize(Ms, RespMessage)
                Ms.Position = 0
                Using Sr As New IO.StreamReader(Ms)
                    Journal.Response = Sr.ReadToEnd()
                End Using
            End Using

            Dim AppConfig As CStandardAppConfig = CStandardAppConfig.GetInstance()
            Dim DBFactory As New CDatabaseFactory(AppConfig.Database())
            Try
                CLog.Info("Update Journal")
                Journal.Update(DBFactory)
                CLog.Info("Commit : {0}", Journal.JournalId)

                If Response.Action = BancslinkTxnResponseBase.CAction.SUP Then
                    ' Insert Superviosr Override Information
                    For Each OverrideDetail As BancslinkTxnResponseBase.COverrideDetail In Response.OverrideDetails
                        Dim OverrideHistory As New COverrideHistory With {.OverrideCode = OverrideDetail.OverrideCode,
                                                                          .OverrideId = Response.OverrideId,
                                                                          .UserId = UserId}
                        OverrideHistory.Insert(DBFactory)
                    Next

                End If
                DBFactory.DatabaseAdapter.Commit()
                CLog.Info("Commit OK")
            Catch ex As Exception
                CLog.Warning("Update Journal Fail {0}", ex.Message)
                DBFactory.DatabaseAdapter.Rollback()
            Finally
                DBFactory.DatabaseAdapter.Close()
            End Try
        End If
    End Sub
    Async Function OverrideCheckAsync(Code As String, _
                                      Screen As Generic.Dictionary(Of String, String), _
                                      UserInfo As Generic.Dictionary(Of String, String)) As Threading.Tasks.Task(Of String)
        Return Await Threading.Tasks.Task.Run(Of String)(
            Function()
                Try
                    Dim MyType As Type = OverrideCode.Item(Code).OverrideType
                    Dim OvCheck As IOverrideCheck = Activator.CreateInstance(MyType)
                    ' Dynamic Call
                    If OvCheck.Check(Screen, UserInfo) = True Then
                        Return Code
                    End If

                Catch ex As Exception

                End Try

                Return ""
            End Function)
    End Function
    Public Class BancslinkTxnResponseBase
        Inherits CTransactionBase.CStandardResponseBase
        Property OverrideDetails As New List(Of COverrideDetail)
        Property OverrideId As String = Guid.NewGuid.ToString
        Property HostErrCode As String
        Property HostErrDescription As String

        Public Class COverrideDetail
            Property OverrideCode As String
            Property OverrideDescription As String
            Property Capability As Integer

        End Class
        Property Action As CAction = CAction.OK
        Public Enum CAction As Integer
            [Default] = 0
            [ERROR] = 4
            OK = 8
            SUP = 1
            NewScreen = 3
        End Enum

        Public Overrides Function EncodeError() As Library.Data.CMessage
            Dim Message As Library.Data.CMessage = MyBase.EncodeError()
            If Action <> CAction.SUP Then
                Action = CAction.ERROR
            End If
            Message.SetValue("Action", CInt(Action))

            If Action = CAction.SUP Then
                Message.SetValue("OverrideId", OverrideId)
                Message.SetValue("OverrideDetails", OverrideDetails)
            End If

            Return Message
        End Function

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As Library.Data.CMessage = MyBase.Encode()
            Message.SetValue("Action", CInt(Action))

            Return Message
        End Function
    End Class
End Class
