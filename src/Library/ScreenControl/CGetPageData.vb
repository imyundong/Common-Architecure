Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports Newtonsoft.Json.Linq

Public Class CGetPageData
    Inherits CWorkflowComponent
    Public Overrides ReadOnly Property Name As String
        Get
            Return "GetPageData"
        End Get
    End Property

    Public Overrides ReadOnly Property PreNotify As String
        Get
            Return "Retrieve Page Data : " + _Properties.PageId
        End Get
    End Property

    Public Overrides Sub Process()
        Dim PageId As String = _Properties.PageId

        If String.IsNullOrEmpty(PageId) Then
            Throw New CError.CBusinessException(CError.CErrorCode.INVALID_PAGE_ID, "Page Id is Null or Empty")
        End If

        Dim Page As New CPageData()
        Dim ConfigFile As String = CUtility.ServerPath("Views") + "\" + PageId + ".cfg.xml"
        CLog.Info("Load Page Config : {0}", ConfigFile)
        Page.Load(ConfigFile)

        If Not String.IsNullOrEmpty(Page.Link) Then
            Using Sr As New IO.StreamReader(CUtility.ServerPath(Page.Link))
                Page.Content = Sr.ReadToEnd
            End Using
        End If

        Page.PageId = PageId
        CLog.Info("Return Page Data")
        _Properties.PageData.Value = JObject.Parse(Page.ToString)
    End Sub

    <Serializable>
    <Xml.Serialization.XmlRoot("PageConfig")>
    Public Class CPageData
        Inherits CXmlConfig
        Implements IConfiguration
        Property PageId As String = ""
        Property Content As String = ""
        Property TransmitButton As Boolean = True
        Property CloseButton As Boolean = False
        Property Title As String
        Property TxnCode As String
        Property Columns As Integer = 1
        Property OnReady As String
        Property Link As String

        <Xml.Serialization.XmlArrayItem("Field")>
        Property Fields As New List(Of CFields)
        Public Class CFields
            <Xml.Serialization.XmlAttribute>
            Property Name As String

            Private _Description As String
            <Xml.Serialization.XmlAttribute>
            Property Description As String
                Get
                    If String.IsNullOrEmpty(_Description) Then
                        Return Name
                    Else
                        Return _Description
                    End If
                End Get
                Set(value As String)
                    _Description = value
                End Set
            End Property
            <Xml.Serialization.XmlAttribute>
            Property InputType As CInputType = CInputType.Input
            <Xml.Serialization.XmlAttribute>
            Property IsMandatory As Boolean = False
            <Xml.Serialization.XmlAttribute>
            Property IsReadonly As Boolean = False
            <Xml.Serialization.XmlAttribute>
            Property FieldType As String = ""
            <Xml.Serialization.XmlAttribute>
            Property Formatter As String = ""
            <Xml.Serialization.XmlAttribute>
            Property Width As Integer
            <Xml.Serialization.XmlAttribute>
            Property ErrorCode As String
        End Class

        Public Enum CInputType As Integer
            Input = 0
            Combobox = 1
        End Enum
        Public Overrides Function ToString() As String
            Return Newtonsoft.Json.JsonConvert.SerializeObject(Me)
        End Function

        Public Sub OnSerialized() Implements IConfiguration.OnSerialized

        End Sub

        Public ReadOnly Property Path As String Implements IConfiguration.Path
            Get
                Return ""
            End Get
        End Property
    End Class

    Public Class CGetPageDataDesigner
        Inherits CWorkflowComponentDesigner
        Property PageId As String
        Property PageData As JProperty
    End Class

    Private _Properties As New CGetPageDataDesigner
    Public Overrides Property Properties As CWorkflowComponentDesigner
        Get
            Return _Properties
        End Get
        Set(value As CWorkflowComponentDesigner)
            _Properties = value
        End Set
    End Property

End Class
