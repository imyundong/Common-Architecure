Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports Newtonsoft.Json.Linq
Public Class CWorkflowDistributor
    Private Shared SyncObject As New Object
    Private Const XmlDeclare As String = "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Public Function Process(ByVal Request As IO.Stream, Optional Encoding As Text.Encoding = Nothing) As CWorkflowData
        Dim ServerDate As Date = Now()
        ' Start System
        ColdStart()
        ' Decode Request
        Dim FlowData As CWorkflowData
        Try
            CLog.Info("Deserilise Request")
            CLog.Info("Text Encoding : {0}", Encoding.ToString)
            CLog.Dump(Request)

            If Encoding Is Nothing Then Encoding = Text.Encoding.UTF8
            Using Sr As New IO.StreamReader(Request, Encoding)
                Dim RequestContext As String = Sr.ReadToEnd()
                FlowData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CWorkflowData)(RequestContext)
            End Using

            ' Try to Decode
            If FlowData Is Nothing Then
                Throw New Exception("Request is Empty")
            Else
                ' Setup System Date
                FlowData.Control.ServerDate = ServerDate
            End If
        Catch ex As Exception
            CLog.Err("Invalid Request", ex)
            Throw New CError.CBusinessException(CError.CErrorCode.INVALID_REQUEST, ex)
        End Try

        Try
            Process(FlowData)
        Catch ex As CError.CBusinessException
            If Not String.IsNullOrEmpty(ex.SystemId) Then
                FlowData.Control.SystemId = ex.SystemId
            End If

            FlowData.Control.ErrCode = ex.ErrCode
            FlowData.Control.ErrDescription = ex.ErrDescription
        Catch ex As Exception
            FlowData.Control.ErrCode = CError.CErrorCode.INTERNAL_ERROR
            FlowData.Control.ErrDescription = ex.Message
            CLog.Err(ex.Message)
        Finally
            ' Record Total Process Time
            FlowData.Control.ProcTime = (Now - FlowData.Control.ServerDate).TotalMilliseconds
        End Try

        Return FlowData
    End Function
    Private Sub Process(ByRef FlowData As CWorkflowData)
        CLog.Info("Workflow Id   {0}", FlowData.Id)
        CLog.Info("Workflow      {0}", FlowData.Control.FlowId)
        CLog.Info("Workflow Node {0}", FlowData.Control.NodeId)
        CLog.Info("User Id       {{{0}}}, {1}", FlowData.UserInfo.UserId, FlowData.UserInfo.UserRole)

        CLog.Debug("Client Date", FlowData.Control.SystemDate)
        ' Load Workflows
        CLog.Info("Load {0}", FlowData.Control.FlowId)
        Dim Workflow As CWorkflow = CWorkflow.GetInstance(FlowData.Control.FlowId)
        If Workflow Is Nothing Then
            CLog.Err("Invalid Workflow {0}", FlowData.Control.FlowId)
            Throw New CError.CBusinessException(CError.CErrorCode.INVALID_WORKFLOW, "Invalid Workflow {0}", FlowData.Control.FlowId)
        End If

        CLog.Info("Get Workflow Node {{{0}}}", FlowData.Control.NodeId)
        ' Retrieve Workflow Node
        Dim Node As CWorkflow.CWorkflowNode = Workflow.GetNodeById(FlowData.Control.NodeId)
        If Node Is Nothing Then
            CLog.Err("Invalid Workflow Node {{{0}}}", FlowData.Control.NodeId)
            Throw New CError.CBusinessException(CError.CErrorCode.INVALID_WORKFLOW_NODE, "Invalid Workflow Node {0}", FlowData.Control.NodeId)
        End If

        CLog.Sys("Process Node {{{0}}}", FlowData.Control.NodeId)
        CLog.Debug("Is Complete", FlowData.Control.IsComplete)
        CLog.Debug("Node Assembly", Node.Assembly)
        CLog.Debug("Node Component", Node.Component)
        CLog.Debug("Node Run At", Node.RunAt)
        If FlowData.Control.IsComplete = True Then
            ' Reset Complete Status
            FlowData.Control.IsComplete = False
            If Node.RunAt = CWorkflow.CWorkflowNode.CRunAt.Client Then
                ' Clean up Client Notification
                FlowData.Control.Notify = ""
            End If
            CLog.Info("Process Next Node {{{0}}}", Node.NextNode)
            FlowData.Control.NodeId = Node.GetNextNode(FlowData.Control.Outcome)
            Process(FlowData)
            Exit Sub
        End If

        If Node.RunAt = CWorkflow.CWorkflowNode.CRunAt.Client Then
            ' Send to Client
            FlowData.Control.Component = Node.Component

            If Node.Component.ToUpper = "END" Then
                If ProcessLastNode(Node, FlowData) = True Then
                    Process(FlowData)
                End If
            End If

            If Node.NextNode.Count = 1 AndAlso Node.NextNode(0).NextNodeId.ToUpper = "END" Then
                FlowData.Control.IsLastComponent = True
            End If

            Exit Sub
        End If

        CLog.Sys("Create Workflow Component")
        Dim WorkflowComponent As CWorkflowComponent
        Try
            WorkflowComponent = Node.GetNodeInstance()
            If WorkflowComponent Is Nothing Then
                CLog.Err("Fail to Load Workflow Component {{{0}}}", FlowData.Control.NodeId)
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_WORKFLOW_COMPONENT, "Invalid Workflow Node {0}", FlowData.Control.NodeId)
            End If
        Catch ex As Exception
            CLog.Err("Invalid Workflow Component", ex)
            Throw New CError.CBusinessException(CError.CErrorCode.INVALID_WORKFLOW_COMPONENT, ex)
        End Try

        CLog.Sys("Setting Parameters")
        ' Sample <Parameter Id="ScreenData" DataType="Reference" Value="UserData/ScreenData" />
        PrepareComponentParameters(Node, WorkflowComponent, FlowData)

        If (Not String.IsNullOrEmpty(WorkflowComponent.PreNotify)) AndAlso _
            FlowData.Control.IsBackground = False AndAlso _
            String.IsNullOrEmpty(FlowData.Control.Notify) Then
            CLog.Sys("Send Notification to Client")
            FlowData.Control.Notify = Now.ToString("MM-dd HH:mm:ss|") + WorkflowComponent.PreNotify
            Exit Sub
        Else
            FlowData.Control.Notify = ""
        End If

        CLog.Sys("Process")
        Try
            WorkflowComponent.WorkflowData = FlowData
            Dim CalllingStackCounter As Integer = FlowData.Control.WorkflowStack.Count

            Dim CurrentFlowId As String = FlowData.Control.FlowId
            WorkflowComponent.Process()

            CLog.Sys("Process Successful")
            If CurrentFlowId <> FlowData.Control.FlowId Then
                CLog.Info("Jump From {0} to {1}", CurrentFlowId, FlowData.Control.FlowId)
                Process(FlowData)
                Exit Sub
            End If
            ' Resotre Workflow Parameter
            ' For Each Parameter As CWorkflow.CWorkflowNode.CWorkflowNodeParameter In Node.Parameters
            '     RestoreParameter(WorkflowComponent, FlowData, Parameter)
            ' Next
            ' Get Next Node
            If ProcessNextNode(Workflow, FlowData, Node, WorkflowComponent.Outcome) = False Then
                Exit Sub
            End If

            Process(FlowData)
        Catch ex As Exception
            CLog.Err("Business Exception", ex)
            Throw ex
        End Try

    End Sub

    Function ProcessLastNode(ByVal NextNode As CWorkflow.CWorkflowNode,
                             ByVal FlowData As CWorkflowData) As Boolean
        If FlowData.Control.WorkflowStack.Count > 0 Then
            Dim NextFlowId = FlowData.Control.WorkflowStack(FlowData.Control.WorkflowStack.Count - 1)
            ' Retrieve Workflow
            Dim TempFlow As CWorkflow = CWorkflow.GetInstance(NextFlowId)
            FlowData.Control.FlowId = NextFlowId
            ' Retrieve Prev Node
            Dim PrevNodeId As String = FlowData.Control.NodeStack(FlowData.Control.NodeStack.Count - 1)
            Dim PrevNode As CWorkflow.CWorkflowNode = TempFlow.GetNodeById(PrevNodeId)

            ' Remove Flow From Stack
            FlowData.Control.WorkflowStack.RemoveAt(FlowData.Control.WorkflowStack.Count - 1)
            FlowData.Control.NodeStack.RemoveAt(FlowData.Control.NodeStack.Count - 1)

            CLog.Info("Return to Calling Flow {0} : {1}", NextFlowId, FlowData.Control.NodeId)
            ProcessNextNode(TempFlow, FlowData, PrevNode, NextNode.NextNode(0).NextNodeId)
        Else
            CLog.Info("Reach The Last Node")
            FlowData.Control.Component = "End"

            Return False
        End If

        Return True
    End Function
    ' Process The Last Node
    Private Function ProcessNextNode(ByVal Workflow As CWorkflow, ByVal FlowData As CWorkflowData, _
                                ByVal Node As CWorkflow.CWorkflowNode, Outcome As String) As Boolean
        Dim NextNodeId As String = Node.GetNextNode(Outcome)
        CLog.Info("Next Node {{{0}}}", NextNodeId)
        Dim NextFlowId As String = FlowData.Control.FlowId

        ' Check Next Node
        Dim NextNode As CWorkflow.CWorkflowNode = Workflow.GetNodeById(NextNodeId)
        If NextNode.Component.ToUpper = "END" Then
            Return ProcessLastNode(NextNode, FlowData)
        Else
            CLog.Info("Set Next Node {{{0}}}", Node.NextNode)
            FlowData.Control.NodeId = Node.GetNextNode(Outcome)
        End If

        Return True
    End Function

    ' Restore Parameter (From Component to Workflow Data)
    Private Sub RestoreParameter(WorkflowComponent As CWorkflowComponent, _
                             FlowData As CWorkflowData, _
                             Parameter As CWorkflow.CWorkflowNode.CWorkflowNodeParameter)

        'If Parameter.DataType = CWorkflow.CWorkflowNode.CWorkflowNodeParameter.CDataType.Reference Then
        '    Dim Values As String() = Parameter.Value.Split("/")
        '    Dim CurrentToken As Newtonsoft.Json.Linq.JToken = FlowData.UserData

        '    For idx As Integer = 1 To Values.Length - 1
        '        CurrentToken = GetNode(CurrentToken, Values(idx))
        '    Next

        '    Dim TargetToken As Newtonsoft.Json.Linq.JToken = CallByName(WorkflowComponent, Parameter.Id, CallType.Get)
        '    CurrentToken.Replace(TargetToken)
        'End If
    End Sub

    Private Sub PrepareComponentParameters(Node As CWorkflow.CWorkflowNode, _
                                           WorkflowComponent As CWorkflowComponent, _
                                           FlowData As CWorkflowData)
        Dim PropertiesType As Type = WorkflowComponent.Properties.GetType()

        For Each Parameter As CWorkflow.CWorkflowNode.CWorkflowNodeParameter In Node.Parameters
            CLog.Info("Parameter {0, -10: G}, {1, -10:G}, {2}", Parameter.Id, Parameter.DataType, Parameter.Value)

            Try
                Dim Target As Object

                If Parameter.DataType = CWorkflow.CWorkflowNode.CWorkflowNodeParameter.CDataType.Value Then
                    Target = Parameter.Value
                Else
                    Dim Source As JToken
                    If Parameter.Value.StartsWith("@") Then
                        Source = RetriveOrCreateNode(FlowData.UserData, Parameter.Value.Replace("@", ""))
                        Source = RetriveOrCreateNode(FlowData.UserData, DirectCast(Source, JProperty).Value)
                    Else

                        If Parameter.Value.StartsWith("Control") Then
                            Source = New JProperty(Parameter.Value.Split("/")(1), CallByName(FlowData.Control, Parameter.Value.Split("/")(1), CallType.Get, Nothing))
                        ElseIf Parameter.Value.StartsWith("UserInfo") Then
                            Source = New JProperty(Parameter.Value.Split("/")(1), CallByName(FlowData.UserInfo, Parameter.Value.Split("/")(1), CallType.Get, Nothing))
                        Else
                            Source = RetriveOrCreateNode(FlowData.UserData, Parameter.Value)
                        End If
                    End If

                    Dim TargetProperty As Reflection.PropertyInfo = _
                        WorkflowComponent.Properties.GetType.GetProperty(Parameter.Id)

                    If TargetProperty.PropertyType Is GetType(JProperty) OrElse _
                        (TargetProperty.PropertyType Is GetType(JToken) AndAlso DirectCast(Source, JProperty).Value.GetType Is GetType(JObject)) Then
                        Target = Source
                    Else
                        Target = DirectCast(Source, JProperty).Value
                    End If

                End If

                CallByName(WorkflowComponent.Properties, Parameter.Id, CallType.Set, Target)
            Catch ex As Exception
                CLog.Warning("Setting Parameter Fail : {0}", ex.Message)
            End Try
        Next

        CLog.Info("Parameters Set Done")
        
    End Sub

    Private Sub ColdStart()
        If CEnviroment.Status = CSystemStatus.Started Then
            Exit Sub
        End If

        If CEnviroment.Status = CSystemStatus.Failed Then
            CLog.Warning("Last Start Failed, Try to Restart")
        End If

        ' Startup System
        SyncLock SyncObject
            If CEnviroment.Status = CSystemStatus.Started Then
                Exit Sub
            End If

            Try
                CLog.Sys("Restart {0}", CEnviroment.Status)
                Start()
                CEnviroment.Status = CSystemStatus.Started
                CLog.Sys("System ({0})", CEnviroment.Status)
                CLog.Sys("Web Application, {0}", CEnviroment.IsWebApplication)
            Catch ex As Exception
                ' CLog.Err("System Started Fail", ex)
                CEnviroment.Status = CSystemStatus.Failed
                Throw ex
            End Try

        End SyncLock
    End Sub

    ' Retrieve Xml Node
    Private Function RetriveOrCreateNode(ByVal Token As JObject, ByVal XPath As String) As JToken
        Dim Nodes As String() = XPath.Split("/")
        Dim Target As JToken = Token

        For i As Integer = 1 To Nodes.Length - 1
            Dim Current As JToken = Target.SelectToken(Nodes(i))
            If Current Is Nothing Then
                Current = New JProperty(Nodes(i), New JObject())

                If Target.GetType Is GetType(JObject) Then
                    DirectCast(Target, JObject).Add(Current)
                Else
                    DirectCast(Target, JProperty).Add(Current)
                End If
                Target = DirectCast(Current, JProperty).Value
            Else
                Target = Current
            End If

        Next

        Return Target.Parent
    End Function

    Private Sub Start()
        ' Setup Enviorment Path
        CEnviroment.IsWebApplication = True
        ' Load System Parameter
        Dim SysConfig As CSystemConfig
        Try
            SysConfig = CSystemConfig.GetInstance()
            ' Initial Log
            CLog.Init(SysConfig.LogInfo)

            CLog.Sys("Start System")
            CLog.Info("Status : {0}", CEnviroment.Status)
            CLog.Info("Log Initial Successful")
            CLog.Debug("Log Size", SysConfig.LogInfo.LogSize)
            CLog.Debug("Path", SysConfig.LogInfo.LogPath)
            CLog.Debug("Log Count", SysConfig.LogInfo.LogCount)
            CLog.Debug("Level", SysConfig.LogInfo.LogLevel)
            CLog.Debug("Workflow Path", SysConfig.Workflow.Path)
            CLog.Info("Communication Component Path {0}", SysConfig.Communication.ComponentPath)
            CLog.Info("Hosts {0}", SysConfig.Communication.Host.Count)

            If SysConfig.Communication.Host.Count = 0 Then
                CLog.Info("Standard Alone Web Application")
            Else
                CLog.Info("Host Id {{{0}}}", SysConfig.Communication.Host(0).Id)
                CLog.Info("Front End System")
                CLog.Info("Message Converter : {0}", SysConfig.Communication.Host(0).Converter)
                CLog.Info("Assembly          : {0}", SysConfig.Communication.Host(0).Component.Assembly)
                CLog.Info("Category          : {0}", SysConfig.Communication.Host(0).Component.Category)
                CLog.Info("Mode              : {0}", SysConfig.Communication.Host(0).Component.Mode)
                CLog.Info("Run At            : {0}", SysConfig.Communication.Host(0).Component.RunAt)
                CLog.Info("Address           : {0}", SysConfig.Communication.Host(0).Component.Address)
                CLog.Info("Port              : {0}", SysConfig.Communication.Host(0).Component.Port)
            End If
        Catch ex As Exception
            CLog.Init(New CLogInfo())
            CLog.Err("Load System Parameter Failed", ex)
            Throw ex
        End Try

        CLog.Sys("Load Communication Component")
        Try
            Dim ComponentPath As String = SysConfig.Communication.ComponentPath
            Dim CommType As Type = CUtility.ComponentLoader(ComponentPath, _
                                                            SysConfig.Communication.Host(0).Component.Assembly, _
                                                            SysConfig.Communication.Host(0).Component.Category, _
                                                            GetType(ICommunication))
            CLog.Sys("Initial Communication {0}", SysConfig.Communication.Host(0).Component.Category)
            Dim Communication As ICommunication = Activator.CreateInstance(CommType)
            Communication.Init(SysConfig.Communication.Host(0))
            ' Save Communication Component
            SysConfig.Com = Communication
        Catch ex As Exception
            CLog.Err("Load Communication Component Fail, Due to {0}", ex.Message)
            Throw ex
        End Try


    End Sub

End Class

