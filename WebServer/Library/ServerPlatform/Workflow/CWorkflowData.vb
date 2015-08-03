Imports Newtonsoft.Json.Linq
Imports ServerPlatform.Library
Imports ServerPlatform.Library.Workflow.CWorkflowComponent

Namespace Workflow
    <Serializable>
    Public Class CWorkflowData
        Property Id As String
        Property Control As New CControler
        Property UserInfo As New CUserInfo
        Property UserData As JObject

        Public Class CUserInfo
            Property UserId As String = ""
            Property UserRole As Integer
            Property Institution As Integer
            Property BranchId As Integer
            Property Terminal As Integer
        End Class

        Public Class CControler
            Property FlowId As String = "StandardFlow"
            Property NodeId As String = "Start"
            Property Component As String = ""
            Property Outcome As String = CCOutcome.DEFAULT_OUTCOME

            Property WorkflowStack As New List(Of String)
            Property NodeStack As New List(Of String)
            Property IsComplete As Boolean = False
            Property IsLastComponent As Boolean = False
            Property ServerDate As Date = Now()
            Property ProcTime As Integer = 0
            Property IsBackground As Boolean = False
            Property SystemDate As DateTime = Now()
            Property ErrDescription As String = ""
            Property ErrCode As Integer

            Private _SystemId As String
            Property SystemId As String
                Get
                    If String.IsNullOrEmpty(_SystemId) Then
                        Return "Bancslink"
                    Else
                        Return _SystemId
                    End If
                End Get
                Set(value As String)
                    _SystemId = value
                End Set
            End Property
            Property Notify As String = ""
        End Class

        Public Overrides Function ToString() As String
            'UserData.InnerXml = UserData.SelectSingleNode("UserData")
            Return Newtonsoft.Json.JsonConvert.SerializeObject(Me)
        End Function
    End Class
End Namespace