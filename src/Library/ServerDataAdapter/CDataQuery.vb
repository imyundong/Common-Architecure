Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports Newtonsoft.Json.Linq
Imports System.Xml.Serialization

Public Class CQuery
    Inherits CWorkflowComponent

    Public Overrides ReadOnly Property Name As String
        Get
            Return "DataQuery"
        End Get
    End Property

    Public Overrides ReadOnly Property PreNotify As String
        Get
            Return "Host Data Query : " + _Properties.ScreenData.Value.SelectToken("TxnCode").ToString
        End Get
    End Property

    Public Overrides Sub Process()
        Dim TxnCode As String = _Properties.ScreenData.Value.SelectToken("TxnCode")

        If String.IsNullOrEmpty(TxnCode) Then
            CLog.Info("Txn Code Is Empty")
            Exit Sub
        End If

        CLog.Info("Server Data Query Process")
        Dim SysConfig As CSystemConfig = CSystemConfig.GetInstance()
        If SysConfig.Com Is Nothing Then
            CLog.Err("Communication is not Available")
            Throw New CError.CBusinessException(CError.CErrorCode.CONNECTION_DISCONNECTED, "Communication is not Available")
        End If

            ' Create Server Message
        Dim Message As New CMessage
            ' Prepare Data
        Message.UserId = WorkflowData.UserInfo.UserId
        Message.SystemId = WorkflowData.Control.SystemId
        Message.SetValue("BranchId", WorkflowData.UserInfo.BranchId)
        Message.SetValue("BranchNo", WorkflowData.UserInfo.Institution)
        Message.SetValue("WorkflowId", WorkflowData.Id)
        CLog.Info("Get Txn Code : {0}", TxnCode)
        Message.TxnCode = TxnCode
            ' Setup User IPAddress
        Message.IPAddress = System.Web.HttpContext.Current.Request.UserHostAddress

        For Each Token As JToken In _Properties.ScreenData.Value.Children
            Try
                Dim Prop As JProperty = Token
                Message.SetValue(Prop.Name, Prop.Value.ToString)
            Catch ex As Exception

            End Try
        Next
        ' Clear Host Data
        _Properties.HostData.Value = New JObject()

        CLog.Info("Send Message : {0}", Message.TxnCode)
        SysConfig.Com.Send(Message)

        CLog.Info("Receiving")
        Dim Response As CMessage = SysConfig.Com.Receive(Message.MessageId)
           
        CLog.Info("Reponse Received {0}", Response.MessageId)
        Dim Action As CAction = CAction.OK
        Response.GetValueByKey("Action", Action)
        ' Check Response Error Code
        If Response.ErrCode <> CError.CErrorCode.SUCCESSFUL Then
            Dim ErrCode As String = ""
            Response.GetValueByKey("ErrCode", ErrCode)
            If Not String.IsNullOrEmpty(ErrCode) Then
                DirectCast(_Properties.HostData.Value, JObject).Add("ErrCode", ErrCode)
                Dim ErrDescription As String = ""
                Response.GetValueByKey("ErrDescription", ErrDescription)
                DirectCast(_Properties.HostData.Value, JObject).Add("ErrDescription", ErrDescription)
            Else
                ' Assign Error Code
                DirectCast(_Properties.HostData.Value, JObject).Add("ErrCode", Response.ErrCode)
                DirectCast(_Properties.HostData.Value, JObject).Add("ErrDescription", GetErrorDescription(CInt(Response.ErrCode).ToString))
            End If

            WorkflowData.Control.SystemId = Response.SystemId
            If Action <> CAction.SUP Then
                Action = CAction.ERROR
            End If
            Outcome = Action.ToString

        End If

        ' Setup Outcome
        Outcome = Action.ToString
        
        ' Append System Info
        Dim UserAgent As String = System.Web.HttpContext.Current.Request.UserAgent
        DirectCast(_Properties.HostData.Value, JObject).Add("OSVersion", GetOSVersion(UserAgent))
        DirectCast(_Properties.HostData.Value, JObject).Add("Device", GetDeviceInfo(UserAgent))
        DirectCast(_Properties.HostData.Value, JObject).Add("UserClient", GetClientType(UserAgent))

        DirectCast(_Properties.HostData.Value, JObject).Add("IPAddress", Response.IPAddress)
        For Idx As Integer = 0 To Response.Keys.Count - 1
            Try
                DirectCast(_Properties.HostData.Value, JObject).Add(New JProperty(Response.Keys(Idx), Response.Values(Idx)))
            Catch ex As Exception
                CLog.Warning(ex.ToString)
            End Try
        Next

        For Idx As Integer = 0 To Response.TableKeys.Count - 1
            Dim j As New JArray
            Dim Table As System.Data.DataTable = Response.TableValues(Idx)

            For Each Row As System.Data.DataRow In Response.TableValues(Idx).Rows
                Dim jObject As New JObject
                For i As Integer = 0 To Table.Columns.Count - 1
                    Dim Value As Object
                    If Row.Item(i).GetType Is GetType(Date) Then
                        Value = DirectCast(Row.Item(i), Date).ToString("yyyy-MM-dd HH:mm:ss")
                    Else
                        Value = Row.Item(i)
                    End If
                    Dim jProperty As New JProperty(Table.Columns(i).ColumnName, Value)
                    jObject.Add(jProperty)
                Next
                j.Add(jObject)
            Next

            DirectCast(_Properties.HostData.Value, JObject).Add(New JProperty(Response.TableKeys(Idx), j))
        Next

        ' Post Process
        ' Load Config
        Try
            If Action <> CAction.OK Then
                Exit Sub
            End If

            Dim PageConfig As New PageConfig
            Dim PageId As String = _Properties.ScreenData.Value.SelectToken("PageId")

            Dim PageConfigPath As String = CUtility.ServerPath("Views") + "\" + PageId + ".cfg.xml"
            PageConfig.Load(PageConfigPath)

            If Not String.IsNullOrEmpty(PageConfig.Action.Rx.NextPage) Then
                Outcome = CAction.Display.ToString
                DirectCast(_Properties.ScreenData.SelectToken("PageId"), JProperty).Value = PageConfig.Action.Rx.NextPage
            End If
        Catch ex As Exception

        End Try
        ' Hard Code for 001010
        'If TxnCode = "001010" And Response.Keys.Count > 50 Then
        '    Outcome = "Print"
        'End If
    End Sub

    <Serializable>
    Public Class PageConfig
        Inherits CXmlConfig
        Implements IConfiguration
        Property Action As Action

        Public Sub OnSerialized() Implements IConfiguration.OnSerialized

        End Sub

        Public ReadOnly Property Path As String Implements IConfiguration.Path
            Get
                Return ""
            End Get
        End Property
    End Class

    Public Class Action
        Property Rx As CRx
        Public Class CRx
            Property NextPage As String = ""
        End Class
    End Class

    Public Enum CAction As Integer
        [Error] = 4
        OK = 8
        SUP = 1
        Display = 3
    End Enum


    Private Function GetClientType(ByVal UserAgent As String) As String
        If UserAgent.Contains("Trident/7.0") AndAlso UserAgent.Contains("like Gecko") Then
            Return "IE11"
        Else
            Return "Other Browser"
        End If
    End Function

    Private Function GetOSVersion(ByVal UserAgent As String) As String
        Dim OSVersion As String = ""
        If UserAgent.Contains("(") Then
            Dim Start As Integer = UserAgent.IndexOf("(")
            Dim Idx As Integer = UserAgent.IndexOf(";", Start)
            OSVersion = UserAgent.Substring(Start + 1, Idx - Start - 1)

            If OSVersion.Contains("Windows") Then
                Dim Versions As String() = OSVersion.Split(" ")
                If Versions.Length >= 2 Then
                    If (Versions(2) = "6.3") Then
                        Return "Windows 10"
                    ElseIf (Versions(2) = "6.2") Then
                        Return "Windows 8/8.1"
                    ElseIf (Versions(2) = "6.1") Then
                        Return "Windows 7"
                    ElseIf (Versions(2) = "6.0") Then
                        Return "Windows Vista"
                    ElseIf (Versions(2) = "5.1") Then
                        Return "Windows XP"
                    Else
                        Return OSVersion
                    End If
                End If
            End If
        End If

        Return ""
    End Function

    Private Function GetDeviceInfo(ByVal UserAgent As String) As String
        If UserAgent.Contains("Android") Then
            Return "Android"
        ElseIf UserAgent.Contains("BlackBerry") Then
            Return "BlackBerry"
        ElseIf UserAgent.Contains("iPad") Then
            Return "iPad"
        ElseIf UserAgent.Contains("iPhone") Then
            Return "iPhone"
        ElseIf UserAgent.Contains("iPod") Then
            Return "iPod"
        ElseIf UserAgent.Contains("IEMobile") Then
            Return "IEMobile"
        End If

        Return "Desktop"
    End Function
    Public Overrides Property Properties As CWorkflowComponentDesigner
        Get
            Return _Properties
        End Get
        Set(value As CWorkflowComponentDesigner)
            _Properties = value
        End Set
    End Property

    Public Class CDataQueryDesigner
        Inherits CWorkflowComponentDesigner
        Property ScreenData As JProperty
        Property HostData As JProperty

    End Class

    Private _Properties As New CDataQueryDesigner
    Private SharedObject As New Object
    Private Function GetErrorDescription(ByVal Code As String) As String
        If ErrorMessage Is Nothing Then
            SyncLock SharedObject
                If ErrorMessage Is Nothing Then
                    ErrorMessage = New CErrorMessageMapping
                    ErrorMessage.Load()
                End If
            End SyncLock
        End If
        
        If ErrorMessage.ErrList.ContainsKey(Code) Then
            Return ErrorMessage.ErrList.Item(Code)
        ElseIf ErrorMessage.ErrList.ContainsKey(CInt(CError.CErrorCode.INTERNAL_ERROR).ToString) Then
            Return ErrorMessage.ErrList.Item(CInt(CError.CErrorCode.INTERNAL_ERROR).ToString)
        End If

        Return "Unknown Error Code"
    End Function

    Private Shared ErrorMessage As CErrorMessageMapping

    <Serializable>
    <XmlRoot("ErrorMessage")>
    Public Class CErrorMessageMapping
        Inherits CXmlConfig
        Implements IConfiguration

        <XmlElement("Error")>
        Property ErrorList As New List(Of CErrorMessage)
        Public Class CErrorMessage
            <XmlAttribute>
            Property Code As String
            <XmlAttribute>
            Property Message As String
        End Class

        <XmlIgnore>
        Property ErrList As New Generic.Dictionary(Of String, String)

        Public Sub OnSerialized() Implements IConfiguration.OnSerialized
            For Each Item In ErrorList
                If ErrList.ContainsKey(Item.Code) Then
                    ErrList.Item(Item.Code) = Item.Message
                Else
                    ErrList.Add(Item.Code, Item.Message)
                End If
            Next
        End Sub

        Public ReadOnly Property Path As String Implements IConfiguration.Path
            Get
                Return CUtility.ServerPath("Config/ErrorMessage.xml")
            End Get
        End Property
    End Class

End Class


Public Class CMenuInfo
    <XmlElement("ul")>
    Property Root As CMenuFolder

    Public Class CMenuFolder

        <XmlElement("li")>
        Property MenuItem As New List(Of CMenuItem)

        <XmlElement("ul")>
        Property MenuFolder As New List(Of CMenuFolder)
    End Class

    Public Class CMenuItem

        <XmlElement("a")>
        Property MenuLink As CMenuItemLink
        <XmlElement("ul")>
        Property MenuFolder As New List(Of CMenuFolder)

        Public Class CMenuItemLink
            <XmlAttribute("href")>
            Property Link As String
            <XmlAttribute("title")>
            Property Title As String
            <XmlElement("img")>
            Property Icon As CMenuIcon
            <XmlElement("span")>
            Property Value As String
            <XmlElement("b")>
            Property More As String


        End Class

        Public Class CMenuIcon
            Sub New()

            End Sub
            Sub New(Index As Integer)
                Source = "Icons/Icon" + Index.ToString + ".png"
            End Sub

            <XmlAttribute("src")>
            Property Source As String

        End Class
    End Class

End Class

