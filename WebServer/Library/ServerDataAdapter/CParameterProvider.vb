Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports Newtonsoft.Json.Linq

Public Class CParameterProvider
    Inherits CWorkflowComponent

    Public Overrides ReadOnly Property Name As String
        Get
            Return "ParameterProvider"
        End Get
    End Property

    Public Overrides Sub Process()
        CLog.Info("Set Data")
        Dim SystemConfig As CSystemConfig = CSystemConfig.GetInstance()
        Dim JsonString As String = Newtonsoft.Json.JsonConvert.SerializeObject(SystemConfig.SSOSetting)
        Dim JsonSetting As JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(Of JObject)(JsonString)

        Dim Obj As JObject = DirectCast(_Properties.Parameter.Value, JObject)
        Dim SSOSetting As JObject = Obj.SelectToken("SSOSetting")

        If SSOSetting Is Nothing Then
            Obj.Add(New JProperty("SSOSetting", JsonSetting))
            SSOSetting = Obj.SelectToken("SSOSetting")
        End If
        DirectCast(SSOSetting.Parent, JProperty).Value = JsonSetting
    End Sub

    Public Overrides Property Properties As CWorkflowComponentDesigner
        Get
            Return _Properties
        End Get
        Set(value As CWorkflowComponentDesigner)
            _Properties = value
        End Set

    End Property

    Public Class CParameterProviderDesigner
        Inherits CWorkflowComponentDesigner
        Property Parameter As JProperty
    End Class

    Private _Properties As New CParameterProviderDesigner
End Class

