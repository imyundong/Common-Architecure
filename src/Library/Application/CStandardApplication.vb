Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

<Serializable>
Public Class CStandardApplication
    Inherits MarshalByRefObject
    Implements IComponent
    Implements IApplication
    ' Hard Code for Now
    Private WithEvents Connection As ICommunication
    ' Always Alive
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "StandardApplication"
        End Get
    End Property

    Private _AppConfig As New CStandardAppConfig

    Public ReadOnly Property AppConfig As CApplicationConfig Implements IApplication.AppConfig
        Get
            Return _AppConfig
        End Get
    End Property

    Private Hosts As New Generic.Dictionary(Of String, ICommunication)

    Public Sub Start(ByRef AppInfo As CAppConfig.CApplicationInfo) Implements IApplication.Start
        CLog.Sys("Initial Communication Component")
        CLog.Info("Component : {0}.{1}", _AppConfig.Request.Component.Assembly, _AppConfig.Request.Component.Category)
        CLog.Info("Run At    : {0}", _AppConfig.Request.Component.RunAt)
        CLog.Info("Timeout   : {0}", _AppConfig.Request.Component.Timeout)

        Try
            ' Initialise Txn Mapping
            For Each Mapping As CStandardAppConfig.CTxnMapping In _AppConfig.TxnMapping
                If Not _AppConfig.TxnMappingDictionary.ContainsKey(Mapping.TxnCode) Then
                    _AppConfig.TxnMappingDictionary.Add(Mapping.TxnCode, Mapping.MapTo)
                End If
            Next

            Dim Name As String = _AppConfig.Request.Component.Assembly + "." + _AppConfig.Request.Component.Category
            If Not AppConfig.ComponentList.ContainsKey(Name.ToUpper) Then
                CLog.Err("Initial Request Communcation Fail")
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_COMMUNICATION_COMPONENT, "Invalid Communication Communication : {0}", Name)
            End If
            Connection = Activator.CreateInstance(AppConfig.ComponentList.Item(Name.ToUpper))
            Connection.Init(_AppConfig.Request)
            ' TODO
            CLog.Info("Communication Intial Successful")
            ' Initial Host Connections
            For Each Server In _AppConfig.Hosts
                CLog.Info("Host Id : {0}, {1} {2} ", Server.Id, Server.Component, Server.Converter)
                CLog.Info("Address : {0}, {1}", Server.Component.Address, Server.Component.Port)
                Try
                    ' Create Host Connection Instance
                    Dim HostConnection As ICommunication = Activator.CreateInstance _
                                                           (AppConfig.ComponentList.Item((Server.Component.Assembly + "." + Server.Component.Category).ToUpper))
                    HostConnection.Init(Server)
                    Hosts.Add(Server.Id, HostConnection)
                    CLog.Info("{0} Connetion Initialised", Server.Id)
                Catch ex As Exception
                    CLog.Warning("Connection Fail : {0}", ex.Message)
                End Try

            Next

            ' Execute Pre Executed Transaction to Initialise Parameters
            If _AppConfig.TxnList.ContainsKey(CTransactionBase.PRE_EXECUTED_TXN) Then
                Dim Message As New CMessage
                Message.TxnCode = CTransactionBase.PRE_EXECUTED_TXN
                Message.IsAdviced = True
                CLog.Info("Process Pre Executed Transaction")
                Process(Message)
            End If

            CLog.Info("Listening...")
            Connection.BeginReceive()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub Process(ByVal Message As CMessage) Handles Connection.MessageReceived
        Dim Sw As New Stopwatch()
        Sw.Start()
        ' Increase Transactions
        AppStatus.IncreaseTxnCount()

        Dim Response As CTransactionBase.CStandardResponseBase = Nothing
        Dim RespMessage As CMessage = Nothing
        Dim ErrCode As CError.CErrorCode = CError.CErrorCode.SUCCESSFUL
        Dim ErrDescription As String = ""

        Try
            CLog.Info("Request Receive : {0}", Message.TxnCode)
            Dim TxnClassName As String = Message.TxnCode.ToString
            If _AppConfig.TxnMappingDictionary.ContainsKey(TxnClassName) Then
                TxnClassName = _AppConfig.TxnMappingDictionary.Item(TxnClassName)
                CLog.Info("Transation Mapped To : {0}", TxnClassName)
            End If

            If Not AppConfig.TxnList.ContainsKey(TxnClassName.ToUpper) Then
                CLog.Err("Invalid Request,Invalid Txn Code")
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_TRANSACTION_CODE, "Transaction Code Does Not Exist")
            End If

            Dim Txn As CTransactionBase = Activator.CreateInstance(AppConfig.TxnList.Item(TxnClassName.ToUpper))
            ' Set up Client Txn Code
            Txn.ClientTxnCode = Message.TxnCode
            ' Setup Response
            Response = Txn.Response
            Try
                CLog.Info("Decoding")
                Txn.Decode(Message)
                ' Debug
                CLog.Info("Debug")
                Txn.Debug()
                CLog.Info("Processing")

                CLog.Info("Create Database Factory")
                Dim DatabaseFactory As New CDatabaseFactory(_AppConfig.Database)

                Try
                    CLog.Sys("PreProcess")
                    Txn.PreProcess(DatabaseFactory)

                    CLog.Sys("Process")
                    Txn.Process(DatabaseFactory)
                    CLog.Info("Process Successful")

                    If Txn.IsLocal = False Then
                        If String.IsNullOrEmpty(Txn.HostId) Then
                            CLog.Warning("Host Id is not setup.")
                        Else
                            ' Send to Foreign
                            CLog.Sys("Send to Foregin System : {0}", Txn.HostId)
                            Dim Connection As ICommunication = Hosts.Item(Txn.HostId)
                            Dim ForeignMessage As CMessage = Txn.ToForeign(Txn.HostId)
                            Try
                                Connection.Send(ForeignMessage)

                                If ForeignMessage.IsAdviced = False Then
                                    CLog.Sys("Waiting for Foregin System Response : {0}", Txn.HostId)
                                    Try
                                        Dim Index As Integer = 0
                                        Dim IsLastMessage As Boolean = False
                                        While IsLastMessage = False
                                            If Index > 20 Then
                                                CLog.Warning("Infinite Loop Might be Happend")
                                                Throw New CError.CBusinessException(CError.CErrorCode.MESSAGE_NUMBERS_EXCCEED, "Too Many Responses From Host")
                                            End If
                                            CLog.Info("Receive Index : {0}", Index)
                                            Dim ForeignMessageResponse As CMessage = Connection.Receive(ForeignMessage.MessageId)
                                            IsLastMessage = Txn.FromForeign(Index, ForeignMessageResponse)
                                            Index += 1
                                        End While
                                    Catch ex As Exception
                                        Throw ex
                                    End Try
                                End If
                               
                            Catch ex As Exception
                                ' Foregin Txn Fail
                                CLog.Err("Send to Foreign Fail : {0}", ex.Message)
                                Throw ex
                            Finally

                            End Try
                        End If
                    End If

                    CLog.Sys("Commit")
                    If (Not DatabaseFactory.DatabaseAdapter Is Nothing) Then DatabaseFactory.DatabaseAdapter.Commit()
                    CLog.Info("Commit OK")
                Catch ex As Exception
                    CLog.Info("Rollback")
                    If (Not DatabaseFactory.DatabaseAdapter Is Nothing) Then DatabaseFactory.DatabaseAdapter.Rollback()
                    Throw ex
                Finally
                    If (Not DatabaseFactory.DatabaseAdapter Is Nothing) Then DatabaseFactory.DatabaseAdapter.Close()
                End Try
                ' Encode
                If Not Txn.Response Is Nothing Then RespMessage = Response.Encode()

            Catch ex As CError.CBusinessException
                ErrCode = ex.ErrCode
                ErrDescription = ex.Message
                CLog.Err("Process Failed {0}, {1}", ex.ErrCode, ex.Message)
                Throw ex
            Catch ex As Exception
                ErrCode = CError.CErrorCode.INTERNAL_ERROR
                ErrDescription = ex.Message
                CLog.Err("Process Failed, Due to", ex.Message)
                Throw New CError.CBusinessException(CError.CErrorCode.INTERNAL_ERROR, ex)
            Finally
                ' Post Process
                CLog.Info("Post Process")
                Txn.PostProcess(ErrCode, ErrDescription, Sw.ElapsedMilliseconds)
            End Try

        Catch ex As CError.CBusinessException
            ErrCode = ex.ErrCode
            If Message.IsAdviced = False AndAlso Response Is Nothing Then
                Response = New CTransactionBase.CStandardResponseBase
            End If

            If Not Response Is Nothing Then
                CLog.Info("Encoding Error Response")
                Response.ErrCode = ex.ErrCode
                RespMessage = Response.EncodeError()
            End If
        Finally
            If Not RespMessage Is Nothing Then
                RespMessage.OriginalId = Message.MessageId
                CLog.Sys("Send Response Message")
                Try
                    Connection.Send(RespMessage)
                Catch ex As Exception

                End Try
                Sw.Stop()
                AppStatus.DecreaseTxnCount(Sw.ElapsedMilliseconds)
                CLog.Sys("Txn Finished : {0} in {1}s", ErrCode, Sw.Elapsed.TotalSeconds)
            Else
                Sw.Stop()
                AppStatus.DecreaseTxnCount(Sw.ElapsedMilliseconds)
                CLog.Sys("Txn Finished : {0} in {1}s", ErrCode, Sw.Elapsed.TotalSeconds)
            End If

        End Try
    End Sub

    Public Sub [Stop]() Implements IApplication.Stop
        CLog.Info("Stopping Application")
        CLog.Info("Stop Server")
        Connection.StopServices()

        Dim Sw As New Stopwatch
        Sw.Start()
        While AppStatus.TxnCount > 0
            ' Wait for all transaction finished
            CLog.Info("Transaction Still Processing : {0}", AppStatus.TxnCount)
            Threading.Thread.Sleep(1000)
            If Sw.ElapsedMilliseconds > 50000 Then
                ' Over then 30 seconds
                CLog.Warning("Force Close Application")
                Exit While
            End If
        End While
    End Sub

    Public ReadOnly Property BaseTxnType As Type Implements IApplication.BaseTxnType
        Get
            Return GetType(CTransactionBase)
        End Get
    End Property

    Public Property AppStatus As CAppStatus Implements IApplication.AppStatus
End Class
