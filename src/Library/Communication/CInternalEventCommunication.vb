Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports System.Threading
Imports ServerPlatform.Library.Workflow.CError

<Serializable>
Public Class CInternalEventCommunication
    Inherits MarshalByRefObject
    Implements ICommunication
    Implements IComponent

    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "InternalEventComm"
        End Get
    End Property

    ' Component Information
    Private Info As CCommInfo.CHostInfo
    Private IsInitialised As Boolean = False
    ' Global Message
    Private CurrentMessageId As Long = 1
    ' Sync Queue
    Public RequestQ As Queue
    Public ResponseQ As Queue
    ' Set Event After Receiving Message From Queue
    Private RequestList As New Generic.Dictionary(Of String, ManualResetEventSlim)
    ' Store Response Message
    Private ResponseList As New Generic.Dictionary(Of String, CMessage)
    ' Set Event After Insert Message to Response Queue
    Public ReceivedEvent As New AutoResetEvent(False)
    Public MessageSent As New AutoResetEvent(False)
    Public CurrentMessage As CMessage

    Public Sub Init(Info As CCommInfo.CHostInfo) Implements ICommunication.Init
        If IsInitialised = True Then Exit Sub

        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Sync Then _
            Throw New NotImplementedException("Sync mode is not supported, Try Async Mode")
        ' Set Communication Info
        Me.Info = Info
        CLog.Sys("Initial Internal Queue")
        CLog.Info("Host {0}, Converter {1}", Info.Id, Info.Converter)
        CLog.Info("Communication Mode {0}, {1}", Info.Component.Mode, Info.Component.RunAt)

        If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
            CLog.Info("Create Synchronized Queue")
            ' Initialise Queue
            RequestQ = Queue.Synchronized(New Queue)
            ResponseQ = Queue.Synchronized(New Queue)

            CLog.Sys("Listeining to Response Q")
            ' Start Receiving Message
            Dim Task As New System.Threading.Tasks.Task(AddressOf Receive)
            Task.Start()

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
        Else
            CLog.Info("Internal Communication in Server Mode.")
        End If

        IsInitialised = True
    End Sub

    Private Sub Receive()
        If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
            While True
                ReceivedEvent.WaitOne()
                While ResponseQ.Count > 0
                    Try
                        Dim Message As CMessage = ResponseQ.Dequeue
                        CLog.Info("Receive Message From Queue {0}, {1}", Message.MessageId, Message.OriginalId)
                        If RequestList.ContainsKey(Message.OriginalId) Then
                            Dim ManualEvent As ManualResetEventSlim = RequestList.Item(Message.OriginalId)
                            ResponseList.Add(Message.OriginalId, Message)
                            ManualEvent.Set()
                        Else
                            CLog.Warning("Invalid Message")
                        End If
                    Catch ex As Exception
                        CLog.Warning("Receive Fail : {0}", ex.Message)
                    End Try

                End While
            End While
        Else
            ' Run at Server
            Throw New NotImplementedException("Server mode is not implemented")
        End If

    End Sub
    ''' <summary>
    ''' Receive Message From Queue
    ''' </summary>
    ''' <param name="MessageId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Receive(MessageId As String) As CMessage Implements ICommunication.Receive
        If IsInitialised = False Then
            Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                (CErrorCode.COMMUNICATION_NOT_INITIALISED, "Communication is not Initialised")
        End If

        CLog.Sys("Receiving {0}", MessageId)
        If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
            Try
                ' Check Sending List
                Dim ManualEvent As ManualResetEventSlim = RequestList.Item(MessageId)

                Try
                    ' Waite for Response
                    ManualEvent.Wait(Info.Component.Timeout)
                Catch ex As Exception
                    CLog.Warning("Transaction Timeout")
                    Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                        (CErrorCode.MESSAGE_TIME_OUT, "Transaction Timeout")
                Finally
                    ' Remove Event From Request List
                    RequestList.Remove(MessageId)
                End Try

                If ManualEvent.IsSet = False Then
                    CLog.Warning("Transaction Timeout")
                    Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                        (CErrorCode.MESSAGE_TIME_OUT, "Transaction Timeout")
                End If

                CLog.Info("Message Received")
                Dim Message As CMessage = ResponseList.Item(MessageId)
                ' Remove Message From Response List
                ResponseList.Remove(MessageId)

                Return Message
            Catch ex As CBusinessException
                Throw ex
            Catch ex As Exception
                Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                    (CErrorCode.INVALID_MESSAGE_ID, "Invalid Message Id {0}", MessageId)
            End Try

        Else
            ' Run at Server
            Throw New NotImplementedException("Server mode is not implemented")
        End If
    End Function

    Public Function Send(Message As CMessage) As Boolean Implements ICommunication.Send
        If IsInitialised = False Then
            Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                (CErrorCode.COMMUNICATION_NOT_INITIALISED, "Communication is not Initialised")
        End If

        CLog.Sys("Sending")
        If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
            ' Run at Client
            Message.MessageId = Interlocked.Increment(CurrentMessageId)
            Message.SystemDate = Now
            Dim ManualEvent As New ManualResetEventSlim(False)
            ' Add Message to Request List
            RequestList.Add(Message.MessageId, ManualEvent)
            RequestQ.Enqueue(Message)
            CLog.Sys("Sent {0}", Message.MessageId)
            ' Debug Only
            CurrentMessage = Message
            MessageSent.Set()
            Return True
        Else
            ' Run at Server
            CLog.Info("Send Response")
            ResponseQ.Enqueue(Message)
            ReceivedEvent.Set()
            Return True
        End If

    End Function

    Public Sub BeginReceive() Implements ICommunication.BeginReceive
        If Info.Component.RunAt <> CCommInfo.CCommComponentInfo.CRunAt.Server Then
            Throw New NotImplementedException("Begin Receive Only Work Under Server Mode")
        End If

        Dim Task As New System.Threading.Tasks.Task(AddressOf Listener)
        Task.Start()
    End Sub

    Private Sub Listener()
        While True
            MessageSent.WaitOne()
            While RequestQ.Count > 0
                Dim Message As CMessage = RequestQ.Dequeue()
                RaiseEvent MessageReceived(Message)
            End While
        End While
    End Sub


    Public Event MessageReceived(Message As CMessage) Implements ICommunication.MessageReceived
End Class
