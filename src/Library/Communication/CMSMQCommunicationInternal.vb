﻿Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports System.Threading
Imports ServerPlatform.Library.Workflow.CError

Public Class CMSMQCommunicationInternal
    Implements ICommunication
    Implements IComponent

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "MSMQInternal"
        End Get
    End Property

    Public Sub BeginReceive() Implements ICommunication.BeginReceive
        If Info.Component.RunAt <> CCommInfo.CCommComponentInfo.CRunAt.Server Then
            Throw New NotImplementedException("Begin Receive Only Work Under Server Mode")
        End If

        ResponseQ.BeginReceive()
    End Sub

    Private Sub ReceiveComplete(ByVal Sender As Object, ByVal e As System.Messaging.ReceiveCompletedEventArgs) Handles ResponseQ.ReceiveCompleted
        Dim QueueMessage As System.Messaging.Message
        Try
            QueueMessage = RequestQ.EndReceive(e.AsyncResult)
            ' Continue Receiving Message
            ResponseQ.BeginReceive()
        Catch ex As Exception
            CLog.Err("Receiving Message Fail : {0}", ex.Message)
            Try
                ResponseQ.BeginReceive()
            Catch ex1 As Exception
                CLog.Err("Unable to Start MQ : {0}", ex1.Message)
            End Try

            Exit Sub
        End Try
        CLog.Info("Message Received : {0}", QueueMessage.Id)


        Try
            Dim Message As CMessage = QueueMessage.Body
            CLog.Dump(Message)
            Message.MessageId = QueueMessage.Id

            RaiseEvent MessageReceived(Message)
        Catch ex As Exception
            CLog.Warning(ex.Message)
            Exit Sub
        End Try
    End Sub

    Private WithEvents RequestQ As System.Messaging.MessageQueue
    Private WithEvents ResponseQ As System.Messaging.MessageQueue
    Private IsInitialised As Boolean = False

    Private Info As CCommInfo.CHostInfo
    Public Sub Init(CommInfo As CCommInfo.CHostInfo) Implements ICommunication.Init
        If IsInitialised = True Then Exit Sub

        If CommInfo.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Sync Then _
            Throw New NotImplementedException("Sync mode is not supported, Try Async Mode")
        ' Set Communication Info
        Info = CommInfo
        CLog.Sys("Initial Microsoft Message Queue")
        CLog.Info("Host {0}, Converter {1}", Info.Id, Info.Converter)
        CLog.Info("Communication Mode {0}, {1}", Info.Component.Mode, Info.Component.RunAt)
        Dim Address As String() = Info.Component.Address.Split(","c)
        Dim Server As String = Address(0).Trim
        Dim Client As String = Address(1).Trim
        CLog.Info("Server {0}, Client {1} ", Server, Client)
        'If CommInfo.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
        If Not System.Messaging.MessageQueue.Exists(Client) Then
            ' Create Message Queue
            CLog.Sys("Create Message Queue {0}", Client)
            ResponseQ = System.Messaging.MessageQueue.Create(Client)
        End If
        If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
            ' Starting the Fucking App Server
            CLog.Sys("Create AppServerDomain for App Server")
            Dim AppServerDomain As AppDomain = AppDomain.CreateDomain("AppServerDomain", Nothing, CUtility.ServerPath(""), Nothing, False)

            Dim MyType As Type = GetType(AppServer.CSystemCore)

            CLog.Sys("Reflecting App Server Loader")
            Dim AppServerLoader As AppServer.CSystemCore =
                AppServerDomain.CreateInstanceFromAndUnwrap(MyType.Assembly.CodeBase, MyType.FullName)
            CLog.Sys("Start App Server")
            ' Start App Server Inside Seprate Domian
            Try
                Dim ErrCode As CErrorCode = AppServerLoader.Start()
                If ErrCode <> CErrorCode.SUCCESSFUL Then
                    'CLog.Err("Fail to Start App Server : {0}", ErrCode)
                    'CLog.Sys("Unload App Server Domain")
                    'AppDomain.Unload(AppServerDomain)
                    Throw New CBusinessException(ErrCode, "Fail to Start App Server")
                Else
                    CLog.Info("App Server Started")
                End If
            Catch ex As CBusinessException
                CLog.Err("Fail to Start App Server : {0}", ex.ErrCode)
                CLog.Sys("Unload App Server Domain")
                AppDomain.Unload(AppServerDomain)
                Throw ex
            Catch ex As Exception
                CLog.Err("Fail to Start App Server : {0}", ex.Message)
                CLog.Sys("Unload App Server Domain")
                AppDomain.Unload(AppServerDomain)
                Throw ex
            End Try
        End If

        RequestQ = New System.Messaging.MessageQueue(Server)
        ResponseQ = New System.Messaging.MessageQueue(Client)
        RequestQ.MessageReadPropertyFilter.CorrelationId = True
        ResponseQ.MessageReadPropertyFilter.CorrelationId = True
        RequestQ.MessageReadPropertyFilter.Priority = True
        ResponseQ.MessageReadPropertyFilter.Priority = True

        'Else
        '    If Not System.Messaging.MessageQueue.Exists(Server) Then
        '        ' Create Message Queue
        '        CLog.Sys("Create Message Queue {0}", Server)
        '        RequestQ = System.Messaging.MessageQueue.Create(Server)
        '    End If
        '    ResponseQ = New System.Messaging.MessageQueue(Client)
        'End If

        ResponseQ.Formatter = New Messaging.XmlMessageFormatter(New Type() {GetType(CMessage)})
        RequestQ.Formatter = New Messaging.XmlMessageFormatter(New Type() {GetType(CMessage)})
        IsInitialised = True
    End Sub

    Public Event MessageReceived(Message As CMessage) Implements ICommunication.MessageReceived

    Public Function Receive(MessageId As String) As CMessage Implements ICommunication.Receive
        ' Change to Async Later to Improve the Performance
        SyncLock Me
            Try
                Dim Msg As System.Messaging.Message = ResponseQ.ReceiveByCorrelationId(MessageId, New TimeSpan(0, 0, Info.Component.Timeout / 1000))

                CLog.Info("Message Received {0}, Original Id {1}", Msg.Id, MessageId)
                CLog.Dump(Msg.Body)
                Return Msg.Body
            Catch ex As Exception
                CLog.Err("Receive Error : {0}", ex.Message)
                Throw New CBusinessException(CErrorCode.MESSAGE_TIME_OUT, ex.Message)
            End Try
        End SyncLock
    End Function

    Public Function Send(Message As CMessage) As Boolean Implements ICommunication.Send
        If IsInitialised = False Then
            Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                (CErrorCode.COMMUNICATION_NOT_INITIALISED, "Communication is not Initialised")
        End If

        Dim Msg As New System.Messaging.Message
        Msg.Label = Message.TxnCode.ToString + " : " + Now().ToString("yyyyMMdd HH:mm:ss")
        Msg.Body = Message
        CLog.Sys("Send {0}", Message.TxnCode)

        If Not Message.OriginalId Is Nothing Then
            CLog.Info("Original Id : {0}", Message.OriginalId)
            Msg.CorrelationId = Message.OriginalId
        End If

        RequestQ.Send(Msg)

        CLog.Info("Sent {0} to {1}", Msg.Id, RequestQ.Path)

        Message.MessageId = Msg.Id
        Return True
    End Function
End Class
