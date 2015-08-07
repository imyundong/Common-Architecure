Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports Newtonsoft.Json.Linq

Public Class CSetData
    Inherits CWorkflowComponent

    Public Overrides ReadOnly Property Name As String
        Get
            Return "SetData"
        End Get
    End Property

    Public Overrides Sub Process()
        CLog.Info("Set Data")
        '' Field = Value
        If (_Properties.Value IsNot Nothing AndAlso _Properties.Field IsNot Nothing) Then
            If (_Properties.Value.GetType Is GetType(JProperty)) Then
                _Properties.Field.Value = DirectCast(_Properties.Value, JProperty).Value
            Else
                If _Properties.Value.GetType Is GetType(JValue) AndAlso String.IsNullOrEmpty(_Properties.Value.ToString) AndAlso _Properties.Field.Value.GetType Is GetType(JObject) Then
                    _Properties.Field.Value = New JObject
                Else
                    _Properties.Field.Value = _Properties.Value
                End If
            End If
        End If

        If (_Properties.Value1 IsNot Nothing AndAlso _Properties.Field1 IsNot Nothing) Then
            If (_Properties.Value1.GetType Is GetType(JProperty)) Then
                _Properties.Field1.Value = DirectCast(_Properties.Value1, JProperty).Value
            Else
                If _Properties.Value1.GetType Is GetType(JValue) AndAlso String.IsNullOrEmpty(_Properties.Value1.ToString) AndAlso _Properties.Field1.Value.GetType Is GetType(JObject) Then
                    _Properties.Field1.Value = New JObject
                Else
                    _Properties.Field1.Value = _Properties.Value1
                End If
            End If
        End If

        If (_Properties.Value2 IsNot Nothing AndAlso _Properties.Field2 IsNot Nothing) Then
            If (_Properties.Value2.GetType Is GetType(JProperty)) Then
                _Properties.Field2.Value = DirectCast(_Properties.Value2, JProperty).Value
            Else
                If _Properties.Value2.GetType Is GetType(JValue) AndAlso String.IsNullOrEmpty(_Properties.Value2.ToString) AndAlso _Properties.Field2.Value.GetType Is GetType(JObject) Then
                    _Properties.Field2.Value = New JObject
                Else
                    _Properties.Field2.Value = _Properties.Value2
                End If
            End If
        End If

        If (_Properties.Value3 IsNot Nothing AndAlso _Properties.Field3 IsNot Nothing) Then
            If (_Properties.Value3.GetType Is GetType(JProperty)) Then
                _Properties.Field3.Value = DirectCast(_Properties.Value3, JProperty).Value
            Else
                If _Properties.Value3.GetType Is GetType(JValue) AndAlso String.IsNullOrEmpty(_Properties.Value3.ToString) AndAlso _Properties.Field3.Value.GetType Is GetType(JObject) Then
                    _Properties.Field3.Value = New JObject
                Else
                    _Properties.Field3.Value = _Properties.Value3
                End If
            End If
        End If

        If (_Properties.Value4 IsNot Nothing AndAlso _Properties.Field4 IsNot Nothing) Then
            If (_Properties.Value4.GetType Is GetType(JProperty)) Then
                _Properties.Field4.Value = DirectCast(_Properties.Value4, JProperty).Value
            Else
                If _Properties.Value4.GetType Is GetType(JValue) AndAlso String.IsNullOrEmpty(_Properties.Value4.ToString) AndAlso _Properties.Field4.Value.GetType Is GetType(JObject) Then
                    _Properties.Field4.Value = New JObject
                Else
                    _Properties.Field4.Value = _Properties.Value4
                End If
            End If
        End If
        ' Field.Remove()
        ' Prop.Value = Value
    End Sub

    Public Overrides Property Properties As CWorkflowComponentDesigner
        Get
            Return _Properties
        End Get
        Set(value As CWorkflowComponentDesigner)
            _Properties = value
        End Set

    End Property

    Public Class CSetDataDesigner
        Inherits CWorkflowComponentDesigner
        Property Field As JProperty
        Property Value As JToken

        Property Field1 As JProperty
        Property Value1 As JToken

        Property Field2 As JProperty
        Property Value2 As JToken

        Property Field3 As JProperty
        Property Value3 As JToken

        Property Field4 As JProperty
        Property Value4 As JToken

    End Class

    Private _Properties As New CSetDataDesigner
End Class