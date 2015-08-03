Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports Newtonsoft.Json.Linq

Public Class CCondition
    Inherits CWorkflowComponent

    Public Overrides ReadOnly Property Name As String
        Get
            Return "If"
        End Get
    End Property

    Public Overrides Sub Process()
        CLog.Info("Condition If")
        Dim Value As String = ""
        If (_Properties.Value.GetType Is GetType(JProperty)) Then
            Value = DirectCast(_Properties.Value, JProperty).Value
        Else
            Value = _Properties.Value
        End If


        If _Properties.Field.Value = Value AndAlso _Properties.Condition = "=" Then
            Outcome = "True"
        Else
            Outcome = "False"
        End If
    End Sub

    Public Overrides Property Properties As CWorkflowComponentDesigner
        Get
            Return _Properties
        End Get
        Set(value As CWorkflowComponentDesigner)
            _Properties = value
        End Set

    End Property

    Public Class CConditionDesigner
        Inherits CWorkflowComponentDesigner
        Property Field As JProperty
        Property Value As JToken
        Property Condition As String
    End Class

    Private _Properties As New CConditionDesigner
End Class