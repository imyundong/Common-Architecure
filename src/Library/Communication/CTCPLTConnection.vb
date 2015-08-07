Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports System.Threading
Imports ServerPlatform.Library.Workflow.CError
Imports System.Net.Sockets
Imports ServerPlatform.Library.Data.CCommInfo.CCommComponentInfo

Public Class CTCPLTConnection
    Implements ICommunication
    Implements IComponent

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "TCPLTConnection"
        End Get
    End Property

    Public Sub BeginReceive() Implements ICommunication.BeginReceive
        ' Throw New NotImplementedException("Server Mode is not Available")
        Dim Port As Integer = 0

        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Async Then
            Port = Info.Component.LocalPort
        Else
            Port = Info.Component.Port
        End If

        Dim LocalEP As New System.Net.IPEndPoint(Net.IPAddress.Any, Port)
        TCPServer = New TcpListener(LocalEP)
        TCPServer.Start()
        CLog.Info("Begin Receive {0}", Port)
        TCPServer.BeginAcceptTcpClient(New AsyncCallback(AddressOf ConnectionAccpeted), TCPServer)
    End Sub

    Private MessageList As New Dictionary(Of String, CMessage)
    Private Sub ConnectionAccpeted(AsyncResult As IAsyncResult)
        Dim Listener As TcpListener = AsyncResult.AsyncState
        Dim Client As TcpClient = Listener.EndAcceptTcpClient(AsyncResult)
        ' Remove Unconnected Connection
        CheckConnection()
        ' Listen
        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Sync Then
            ' Sync Mode Support Multiple Connections
            CLog.Info("Begin Receive...")
            Listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf ConnectionAccpeted), TCPServer)
        End If

        Dim IPEP As Net.IPEndPoint = DirectCast(Client.Client.RemoteEndPoint, Net.IPEndPoint)
        CLog.Sys("Accept Connection From {0}:{1}", IPEP.Address, IPEP.Port)

        'Dim CurrentIdx As Integer = Threading.Interlocked.Increment(Index)

        Dim Connection As New CTCPConnection()
        'Connection.Index = CurrentIdx

        Connection.TCPClient = Client
        Connection.Status = CTCPConnection.CTCPConnectionStatus.InUse
        ConnectionPool.Add(Connection)
        Client.Client.ReceiveTimeout = Info.Component.Timeout

        While True
            Try
                Dim MsgLen As Integer = Info.Component.MessageLen
                Dim MsgLenBuffer(MsgLen - 1) As Byte
                Dim Length As Integer = Client.Client.Receive(MsgLenBuffer)

                If (Length <> 0) Then
                    Length = Text.Encoding.ASCII.GetString(MsgLenBuffer).Trim()
                    CLog.Sys("Receive Data from Client ({0})", Length)
                    CLog.Dump(MsgLenBuffer)

                    If Info.Component.MessageLenIncluded = True Then
                        Length = Length - Info.Component.MessageLen
                    End If

                    Dim Buffer(Length - 1) As Byte
                    Dim Buf(Length - 1) As Byte
                    Dim BodyLen As Integer = 0
                    Try
                        While BodyLen < Length
                            Dim RecvLen As Integer = Client.Client.Receive(Buffer)
                            ReDim Preserve Buffer(RecvLen - 1)
                            Buffer.CopyTo(Buf, BodyLen)

                            BodyLen += RecvLen
                        End While

                        Dim MsgId As String = Guid.NewGuid().ToString()
                        Connection.MessageId = MsgId
                        CLog.Sys("Message Received, Id {0}", MsgId)
                        CLog.Dump(Buffer)

                        CLog.Sys("Deserilise Message")
                        Dim Message As CMessage = Converter.Deserialise(Buffer)

                        If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Server Then
                            ' Generate GUID for the Message
                            Message.MessageId = MsgId
                        Else
                            ' Run At Client
                            If String.IsNullOrEmpty(Message.MessageId) Then
                                Message.MessageId = MsgId
                            End If
                        End If

                        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Async Then
                            CLog.Info("Message Count : {0}", MessageList.Count)
                            MessageList.Add(Message.MessageId, Message)
                        Else
                            CLog.Sys("Raise Event")
                            RaiseEvent MessageReceived(Message)
                        End If

                    Catch ex As Exception
                        CLog.Warning("Receive Error : {0}", ex)
                        ' Clean up Availables
                        If Client.Client.Available > 0 Then
                            Dim LeftOver(10240) As Byte
                            Client.Client.Receive(LeftOver)
                            CLog.Info("Clean up Left over")
                            CLog.Dump(LeftOver)
                        End If
                    End Try
                Else
                    Connection.Status = CTCPConnection.CTCPConnectionStatus.Close
                    Exit Sub
                End If
            Catch ex As Exception
                CLog.Warning("Invalid Message {0}", ex.Message)
                'If Client.Client.Connected = False Then
                CLog.Info("Connection Closed")
                Connection.Status = CTCPConnection.CTCPConnectionStatus.Close
                Try
                    ' Try to Close Connection
                    Connection.TCPClient.Close()
                Catch ex1 As Exception

                End Try

                ' Start to Listen Unless Running Async Mode
                If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Async Then
                    CLog.Info("Begin Receive...")
                    ' Sync Mode Support Multiple Connections
                    Listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf ConnectionAccpeted), TCPServer)
                End If

                Exit Sub

            End Try
        End While

    End Sub

    Private Sub CheckConnection()
        For Each Item As CTCPConnection In ConnectionPool
            If Item.TCPClient.Client Is Nothing OrElse Item.TCPClient.Client.Connected = False Then
                Item.Status = CTCPConnection.CTCPConnectionStatus.Close
            End If
        Next

        Dim Removed As Integer = 0
        For i As Integer = ConnectionPool.Count - 1 To 0 Step -1
            If ConnectionPool(i).Status = CTCPConnection.CTCPConnectionStatus.Close Then
                CLog.Sys("Remove Broken Connection : {0}", i)
                Try
                    ConnectionPool(i).TCPClient.Client.Close()
                Catch ex As Exception
                    CLog.Sys("Close Connection Error : {0}", ex.Message)
                Finally
                    ConnectionPool.RemoveAt(i)
                End Try
            End If
        Next
    End Sub

    ' Run At Server Sync Mode / Run At Client Async Mode
    Private TCPServer As TcpListener
    ' Run At Client Async Mode
    Private TCPClient As New TcpClient

    Private ConnectionPool As New List(Of CTCPConnection)
    Private IsInitialised As Boolean = False
    Private Converter As IDataConverter
    Private Info As CCommInfo.CHostInfo

    Public Class CTCPConnection
        Property Status As CTCPConnectionStatus = CTCPConnectionStatus.Available
        Public WithEvents TCPClient As TcpClient
        Property MessageId As String
        Public Enum CTCPConnectionStatus As Integer
            Available = 0
            InUse = 1
            Close = 2
        End Enum
    End Class

    Private Shared SyncObject As New Object

    Private Function GetAvaialableConnection() As CTCPConnection
        If Info Is Nothing Then Return Nothing
        SyncLock SyncObject
            For Each Connection As CTCPConnection In ConnectionPool
                If Connection.Status = CTCPConnection.CTCPConnectionStatus.Available Then
                    Connection.Status = CTCPConnection.CTCPConnectionStatus.InUse
                    Return Connection
                End If
            Next

            If ConnectionPool.Count > Info.Component.MaxConnections Then
                Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                               (CErrorCode.REACH_MAX_CONNECTIONS, "Connection Limitation : {0}", Info.Component.MaxConnections)
                Return Nothing
            End If

            Dim NewConnection As New CTCPConnection
            NewConnection.Status = CTCPConnection.CTCPConnectionStatus.InUse
            ConnectionPool.Add(NewConnection)

            Return NewConnection
        End SyncLock
    End Function

    ' Initilise Communication Component
    Public Sub Init(CommInfo As CCommInfo.CHostInfo) Implements ICommunication.Init
        If IsInitialised = True Then Exit Sub
        Me.Info = CommInfo

        Dim AppConfig As CApplicationConfig = CApplicationConfig.GetInstance()
        ' Initialise Data Converter
        Converter = Activator.CreateInstance(AppConfig.ComponentList(CommInfo.Converter.Category.ToUpper))
        Converter.ConverterInfo = CommInfo.Converter

        ' Async Mode Running Client Should Keep Listening to the Port
        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Async Then
            If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
                ' Create New Client Object
                TCPClient = New TcpClient
                ' TODO
                BeginReceive()
            Else
                CLog.Err("TCP Connection Run at Server is not Implemented")
                Throw New NotImplementedException("TCP Connection Run at Server is not Implemented")
            End If
        End If
        IsInitialised = True
    End Sub

    Public Event MessageReceived(Message As CMessage) Implements ICommunication.MessageReceived

    Public Function Receive(MessageId As String) As CMessage Implements ICommunication.Receive
        ' Sync Mode
        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Sync Then
            ' Change to Async Later to Improve the Performance
            For Each Connetion In ConnectionPool
                If Connetion.MessageId.ToString = MessageId Then
                    Try
                        Dim MsgLen(Info.Component.MessageLen - 1) As Byte
                        Connetion.TCPClient.Client.Receive(MsgLen)
                        Dim Len As Integer = Text.ASCIIEncoding.ASCII.GetString(MsgLen).Trim

                        If Info.Component.MessageLenIncluded = True Then
                            Len = Len - Info.Component.MessageLen
                        End If

                        Dim Buffer(Len - 1) As Byte
                        Connetion.TCPClient.Client.Receive(Buffer)
                        CLog.Sys("Received Data From Connection")
                        CLog.Dump(Buffer)
                        CLog.Sys("Convert from Buffer to Internal Object")
                        Dim Message As CMessage = Converter.Deserialise(Buffer)

                        Dim IsLastMessage As Boolean = False
                        Message.GetValueByKey("LastMessageValue", IsLastMessage)

                        If IsLastMessage = True Then
                            Connetion.MessageId = ""
                            Connetion.Status = CTCPConnection.CTCPConnectionStatus.Available
                        End If
                        ' Key Word LastMessageValue
                        Return Message
                    Catch ex As Exception
                        Throw ex
                        ' TEST: Need to Close Connection to Avoid Invalid Message
                        Connetion.MessageId = ""
                        Connetion.Status = CTCPConnection.CTCPConnectionStatus.Close
                    End Try

                End If
            Next
        Else
            ' Async Mode
            Dim Sw As New Stopwatch
            Sw.Start()
            While Sw.ElapsedMilliseconds < Info.Component.Timeout
                Threading.Thread.Sleep(50)
                If MessageList.ContainsKey(MessageId) Then
                    Dim Message As CMessage = MessageList.Item(MessageId)
                    MessageList.Remove(MessageId)
                    CLog.Info("Response Message Retrieved : {0}", MessageId)
                    Return Message
                End If
            End While

            Throw New CBusinessException(CErrorCode.MESSAGE_TIME_OUT, "Time out : {0}", MessageId)
        End If

        Return Nothing
    End Function

    Private Function GetOriginalConnection(ByVal MsgId As String) As CTCPConnection
        For Each Connection As CTCPConnection In ConnectionPool
            If Connection.MessageId = MsgId Then
                Return Connection
            End If
        Next

        Return Nothing
    End Function

    Public Function Send(Message As CMessage) As Boolean Implements ICommunication.Send
        If IsInitialised = False Then
            Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                (CErrorCode.COMMUNICATION_NOT_INITIALISED, "Communication is not Initialised")
        End If
        Dim MessageId As String
        ' Sync Mode
        If Info.Component.Mode = CCommInfo.CCommComponentInfo.CMode.Sync Then
            Dim Connection As CTCPConnection
            If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
                Connection = GetAvaialableConnection()
                If Connection.TCPClient Is Nothing OrElse Connection.TCPClient.Connected = False Then
                    Try
                        CLog.Sys("Connection to {0} : {1}", Info.Component.Address, Info.Component.Port)
                        If Connection.TCPClient Is Nothing Then Connection.TCPClient = New TcpClient
                        Connection.TCPClient.Connect(New System.Net.IPEndPoint(System.Net.IPAddress.Parse(Info.Component.Address), Info.Component.Port))
                    Catch ex As Exception
                        ' Release Connection
                        Connection.Status = CTCPConnection.CTCPConnectionStatus.Available
                        Throw New ServerPlatform.Library.Workflow.CError.CBusinessException _
                            (CErrorCode.COMMUNICATION_NOT_INITIALISED, ex.Message)
                    End Try
                End If

                'Interlocked.Increment(MessageId)
                MessageId = Guid.NewGuid().ToString()
                Connection.MessageId = MessageId
                Message.MessageId = MessageId
            Else
                CLog.Info("Runat Server, Get The Original Connection")
                Try
                    Connection = GetOriginalConnection(Message.OriginalId)
                Catch ex As Exception
                    CLog.Err("Receive Error {0}", ex.Message)
                    Throw ex
                End Try
            End If

            Dim Buffer As IO.MemoryStream = Converter.Serialise(Message)
            Dim TotalLength As Integer = Buffer.Length
            If Info.Component.MessageLenIncluded = True Then
                TotalLength += Info.Component.MessageLen
            End If

            Dim MsgLen As Byte() = Text.Encoding.ASCII.GetBytes(TotalLength.ToString(New String("0", Info.Component.MessageLen)))
            CLog.Dump(MsgLen)
            CLog.Dump(Buffer)

            Dim len As Integer = Text.Encoding.Default.GetString(MsgLen)
            Dim body As String = Text.Encoding.Default.GetString(Buffer.ToArray)
            ' Clear Left Over Message
            If Connection.TCPClient.Client.Available > 0 Then
                Dim Buf(Connection.TCPClient.Client.Available - 1) As Byte
                Connection.TCPClient.Client.Receive(Buf)
            End If

            'Connection.TCPClient.Client.Send(MsgLen)
            'Connection.TCPClient.Client.Send(Buffer.ToArray)

            ' 挡板有问题
            Dim NewBuffer(MsgLen.Length + Buffer.Length - 1) As Byte
            MsgLen.CopyTo(NewBuffer, 0)
            Buffer.ToArray.CopyTo(NewBuffer, MsgLen.Length)
            Connection.TCPClient.Client.Send(NewBuffer)

            CLog.Info("Sent {0} to {1}:{2}", Message.MessageId, Info.Component.Address, Info.Component.Port)
        Else
            SyncLock AsyncObject
                ' Async Mode
                ' RunAt Client
                If Info.Component.RunAt = CCommInfo.CCommComponentInfo.CRunAt.Client Then
                    ' If TCPClient Is Nothing Then TCPClient = New TcpClient()
                    If TCPClient.Connected = False Then
                        Try
                            ' Close Previous Connection
                            TCPClient.Close()
                        Catch ex As Exception

                        End Try
                        ' Build connection
                        TCPClient = New TcpClient
                        TCPClient.Connect(New Net.IPEndPoint(Net.IPAddress.Parse(Info.Component.Address), Info.Component.Port))
                    End If
                    ' Is it necessary to break the connection in case tcpclient is not established.


                    ' Check Message Id In Case Message Id Is Not Defined
                    If String.IsNullOrEmpty(Message.MessageId) Then
                        MessageId = Guid.NewGuid().ToString()
                        Message.MessageId = MessageId
                    End If

                    Dim Buffer As IO.MemoryStream = Converter.Serialise(Message)
                    Dim TotalLength As Integer = Buffer.Length
                    If Info.Component.MessageLenIncluded = True Then
                        TotalLength += Info.Component.MessageLen
                    End If

                    Dim MsgLen As Byte() = Text.Encoding.ASCII.GetBytes(TotalLength.ToString(New String("0", Info.Component.MessageLen)))
                    CLog.Dump(MsgLen)
                    CLog.Dump(Buffer)
                    ' Clear Left Over Message
                    If TCPClient.Client.Available > 0 Then
                        Dim Buf(TCPClient.Client.Available - 1) As Byte
                        TCPClient.Client.Receive(Buf)
                    End If

                    TCPClient.Client.Send(MsgLen)
                    TCPClient.Client.Send(Buffer.ToArray)

                    CLog.Info("Sent {0} to {1}:{2}", Message.MessageId, Info.Component.Address, Info.Component.Port)
                Else
                    CLog.Err("TCP Connection Run at Server is not Implemented")
                    Throw New NotImplementedException("TCP Connection Run at Server is not Implemented")
                End If
            End SyncLock
        End If
        Return True
    End Function

    Private AsyncObject As New Object

    Public Sub Close() Implements ICommunication.Close
        CLog.Sys("Clean Up Connections")
        IsInitialised = False
        If Info.Component.Mode = CMode.Sync Then
            Dim idx As Integer = 0

            For Each Connection In ConnectionPool
                idx += 1
                Try
                    CLog.Sys("Closing Connections : {0}", idx)
                    Connection.TCPClient.Close()
                Catch ex As Exception
                    CLog.Warning(ex.ToString)
                End Try
            Next
        End If

        Try
            TCPClient.Close()
            ConnectionPool.RemoveRange(0, ConnectionPool.Count)
        Catch ex As Exception

        End Try

        ' Remove All Connections
    End Sub

    Public Sub StopServices() Implements ICommunication.StopServices
        CLog.Info("Stop Listening")

        If Info.Component.RunAt = CRunAt.Server Then
            ' Stop Receiving
            Try
                TCPServer.Stop()
            Catch ex As Exception

            End Try
        Else
            ' Run At Client
        End If
    End Sub
End Class
