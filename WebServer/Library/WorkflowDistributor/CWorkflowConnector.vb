Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports Newtonsoft.Json.Linq
Imports System.Xml.Serialization

Public Class CWorkflowConnector
    Inherits CWorkflowComponent

    Public Overrides ReadOnly Property Name As String
        Get
            Return "WorkflowConnector"
        End Get
    End Property

    Public Overrides Sub Process()
        WorkflowData.Control.WorkflowStack.Add(WorkflowData.Control.FlowId)
        WorkflowData.Control.NodeStack.Add(WorkflowData.Control.NodeId)

        Dim Prop As CWorkflowConnectorProperties = Properties

        WorkflowData.Control.FlowId = Prop.Workflow
        WorkflowData.Control.NodeId = Prop.Node
    End Sub

    Private _Properties As New CWorkflowConnectorProperties
    Public Overrides Property Properties As CWorkflowComponentDesigner
        Get
            Return _Properties
        End Get
        Set(value As CWorkflowComponentDesigner)
            _Properties = value
        End Set
    End Property

    Public Class CWorkflowConnectorProperties
        Inherits CWorkflowComponentDesigner
        Property Workflow As String
        Property Node As String
    End Class
End Class
