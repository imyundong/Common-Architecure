Imports ServerPlatform.Library.Utility
Namespace Workflow
    Public MustInherit Class CWorkflowComponent
        Implements IComponent

        Public Class CCOutcome
            Public Const DEFAULT_OUTCOME As String = "DEFAULT_OUTCOME"
            Public Const OUTCOME_TRUE As String = "TRUE"
            Public Const OUTCOME_FALSE As String = "FALSE"
        End Class

        <Xml.Serialization.XmlIgnore>
        Property WorkflowData As CWorkflowData
        Public MustOverride Property Properties As CWorkflowComponentDesigner
        Public MustOverride Sub Process()
        Public Property Outcome As String = CCOutcome.DEFAULT_OUTCOME
        Public MustOverride ReadOnly Property Name As String Implements IComponent.Name
        Overridable ReadOnly Property PreNotify As String
            Get
                Return Nothing
            End Get
        End Property
        Overridable ReadOnly Property PostNotify As String
            Get
                Return Nothing
            End Get
        End Property
    End Class
End Namespace