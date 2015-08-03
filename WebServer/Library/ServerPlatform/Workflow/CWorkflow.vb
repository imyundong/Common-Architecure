Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports System.Xml.Serialization

Namespace Workflow
    <XmlRoot("Workflow")>
    Public Class CWorkflow
        Inherits CXmlConfig
        Implements IConfiguration

        ' Workflow Nodes List
        <XmlElement>
        Public Property Node As New List(Of CWorkflowNode)

        Private _NodeList As New Generic.Dictionary(Of String, CWorkflowNode)
        Public Function GetNodeById(ByVal NodeId As String) As CWorkflowNode
            If _NodeList.ContainsKey(NodeId) Then
                Return _NodeList.Item(NodeId)
            Else
                Return Nothing
            End If
        End Function

        Public Class CWorkflowNode
            Private Shared AssemblyList As New Generic.Dictionary(Of String, Reflection.Assembly)
            Private Shared NodeList As New Generic.Dictionary(Of String, Type)
            Private Shared SyncObject As New Object

            Public Function GetNodeInstance() As CWorkflowComponent
                Dim Node As CWorkflowComponent = CheckWorkflowNode()
                If Not Node Is Nothing Then Return Node

                ' Load Workflow Node From Assembly 
                CLog.Sys("Load Workflow Node From Assembly")
                SyncLock SyncObject
                    ' Check Again
                    Node = CheckWorkflowNode()
                    If Not Node Is Nothing Then Return Node

                    Dim SysConfig As CSystemConfig = CSystemConfig.GetInstance()
                    Dim Path As String = CUtility.ServerPath(SysConfig.Workflow.ComponentPath) + IO.Path.DirectorySeparatorChar + Assembly.ToUpper + ".dll"

                    CLog.Sys("Load Assembly From {{{0}}}", Path)
                    Dim CurrentAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(Path)
                    Dim Count As Integer = 0
                    For Each CustomerizedType As Type In CurrentAssembly.GetTypes
                        If (Not CustomerizedType.IsAbstract) AndAlso IsWorkflowComponent(CustomerizedType) = True Then
                            CLog.Info("Found Workflow Component {0}", CustomerizedType.FullName)
                            Dim WorkflowComponent As CWorkflowComponent = Activator.CreateInstance(CustomerizedType)

                            Dim Fullname As String = Assembly.ToUpper + "_" + WorkflowComponent.Name
                            CLog.Info("Workflow Component Name {{{0, -15:G} }}, Fullname {{{1}}}", WorkflowComponent.Name, Fullname)
                            ' Add Workflow Node in Cache
                            NodeList.Add(Fullname, CustomerizedType)
                            Count += 1
                        End If
                    Next
                    AssemblyList.Add(Assembly.ToUpper, CurrentAssembly)
                    CLog.Sys("Load Assembly {{{0}}} Successful, {1} Node(s) Loaded", Assembly, Count)
                End SyncLock

                Return CheckWorkflowNode()
            End Function

            Private Function IsWorkflowComponent(ByVal MyType As Type) As Boolean
                If MyType.BaseType Is Nothing Then
                    Return False
                End If

                If MyType.BaseType Is GetType(CWorkflowComponent) Then
                    Return True
                Else
                    Return IsWorkflowComponent(MyType.BaseType)
                End If
            End Function

            ' Get Workflow Node From Cache
            Private Function CheckWorkflowNode() As CWorkflowComponent
                Dim Node As String = Assembly.ToUpper + "_" + Component
                If NodeList.ContainsKey(Node) Then
                    CLog.Sys("Create Instance For Workflow Component {{{0}}}", Node)
                    Return Activator.CreateInstance(NodeList.Item(Node))
                End If

                If AssemblyList.ContainsKey(Assembly.ToUpper) Then
                    CLog.Err("Assembly is Loaded, No Such Workflow Node")
                    Throw New NotImplementedException(String.Format("Invalid Workflow Node, Can Not Find {0} Node in Assembly {1}", NodeId, Assembly.ToUpper))
                End If

                Return Nothing
            End Function

            <XmlAttribute>
            Public Property NodeId As String = "Start"
            <XmlAttribute>
            Public Property Assembly As String = ""
            <XmlAttribute>
            Public Property Component As String = ""
            <XmlAttribute>
            Public Property RunAt As CRunAt = CRunAt.Server

            Public Function GetNextNode(ByVal Outcome As String) As String
                Dim DefaultNode As String = "End"

                If String.IsNullOrEmpty(Outcome) Then
                    Outcome = CWorkflowComponent.CCOutcome.DEFAULT_OUTCOME
                End If

                For Each Item As CNextNode In NextNode
                    If Item.Outcome.ToUpper = Outcome.ToUpper Then
                        Return Item.NextNodeId
                    End If

                    If Item.Outcome = CWorkflowComponent.CCOutcome.DEFAULT_OUTCOME Then
                        DefaultNode = Item.NextNodeId
                    End If
                Next

                Return DefaultNode
            End Function

            <XmlElement>
            Public Property NextNode As List(Of CNextNode)

            Public Class CNextNode
                <XmlAttribute>
                Property Outcome As String = CWorkflowComponent.CCOutcome.DEFAULT_OUTCOME
                <XmlText>
                Property NextNodeId As String = "End"
            End Class

            Public Enum CRunAt As Integer
                Server = 0
                Client = 1
            End Enum

            <XmlElement("Parameter")>
            Property Parameters As New List(Of CWorkflowNodeParameter)
            Public Class CWorkflowNodeParameter
                ' <Parameter Id="ScreenData" DataType="Reference" Value="UserData/ScreenData" />\
                <XmlAttribute>
                Property Id As String = ""
                <XmlAttribute>
                Property DataType As CDataType = CDataType.Value
                <XmlAttribute>
                Property Value As String = ""
                Public Enum CDataType As Integer
                    Value = 0
                    Reference = 1
                    ReferenceIn = 2
                End Enum
            End Class
        End Class



        Private Shared WorkflowList As New Generic.Dictionary(Of String, CWorkflow)
        Private Shared SyncObject As New Object
        Public Shared Function GetInstance(ByVal FlowId As String) As CWorkflow
            If WorkflowList.ContainsKey(FlowId.ToUpper) Then
                Return WorkflowList.Item(FlowId.ToUpper)
            End If

            SyncLock SyncObject
                If WorkflowList.ContainsKey(FlowId.ToUpper) Then
                    Return WorkflowList.Item(FlowId.ToUpper)
                End If

                Dim SysConfig As CSystemConfig = CSystemConfig.GetInstance()

                Dim Filename As String = CUtility.ServerPath(SysConfig.Workflow.Path) + IO.Path.DirectorySeparatorChar + FlowId + ".xml"
                CLog.Info("Retrive Workflow From File {0}", Filename)

                Try
                    Dim Workflow As New CWorkflow
                    If Workflow.Load(Filename) Is Nothing Then
                        Throw New Exception("Invalid Workflow")
                    End If
                    CLog.Sys("Load Workflow Successful {{{0}}} Nodes Count {1}", FlowId, Workflow.Node.Count)
                    WorkflowList.Add(FlowId.ToUpper, Workflow)
                    Return Workflow
                Catch ex As Exception
                    CLog.Err("Fail to Load Workflow ", ex)
                    Return Nothing
                End Try

            End SyncLock

        End Function

        Public Sub OnSerialized() Implements IConfiguration.OnSerialized
            For Each NodeItem As CWorkflowNode In Node
                _NodeList.Add(NodeItem.NodeId, NodeItem)
            Next
        End Sub

        Public ReadOnly Property Path As String Implements IConfiguration.Path
            Get
                Return ""
            End Get
        End Property
    End Class
End Namespace