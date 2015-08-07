Imports System.Xml.Serialization
Imports ServerPlatform.Library.Utility

Namespace Data
    <Xml.Serialization.XmlRoot("AppConfig")>
    <Serializable>
    Public Class CAppConfig
        Inherits CXmlConfig
        Implements IConfiguration

#Region "Properties"
        <Xml.Serialization.XmlElement("Log")>
        Property LogInfo As New CLogInfo
        Property PlatformComponent As New CPlatformComponent
        <XmlElement("Application")>
        Property ApplicationConfig As New CApplicationConfig
        <XmlIgnore>
        Property CachedComponentList As New Generic.Dictionary(Of String, Type)
        <Serializable>
        Public Class CApplicationConfig
            <XmlElement("Application")>
            Property Appliations As New List(Of CApplicationInfo)
        End Class

        <Serializable>
        Public Class CApplicationInfo
            Inherits MarshalByRefObject
            <XmlAttribute>
            Property Id As String = ""
            <XmlAttribute>
            Property Category As String = ""
            <XmlAttribute>
            Property ConfigFile As String = ""
            <XmlAttribute>
            Property TxnPath As String = ""
        End Class

        <Serializable>
        Public Class CPlatformComponent
            Property Communication As New CComponentInfo
            Property Application As New CComponentInfo
            Property Database As New CComponentInfo
        End Class

        <Serializable>
        Public Class CComponentInfo
            Property ComponentPath As String = "Component"
            <XmlElement("Component")>
            Property ComponentList As New List(Of CComponentDetail)
        End Class

        <Serializable>
        Public Class CComponentDetail
            <Xml.Serialization.XmlAttribute()>
            Property Assembly As String = ""
            <Xml.Serialization.XmlAttribute()>
            Property Category As String = ""
        End Class
#End Region

        Public Sub OnSerialized() Implements IConfiguration.OnSerialized

        End Sub


        Public ReadOnly Property Path As String Implements IConfiguration.Path
            Get
                Return "Config\AppConfig.xml"
            End Get
        End Property

        Private Shared AppConfig As CAppConfig
        Private Shared SyncObject As New Object

        Public Shared Function GetInstance(Optional ByVal Path As String = Nothing) As CAppConfig
            If Not String.IsNullOrEmpty(Path) Then
                ' Load/Reload Parameters
                AppConfig = Nothing
            End If

            If Not AppConfig Is Nothing Then
                Return AppConfig
            End If

            SyncLock SyncObject
                If Not AppConfig Is Nothing Then
                    Return AppConfig
                End If

                Dim SysConfig As New CAppConfig
                SysConfig.Load("Config" + IO.Path.DirectorySeparatorChar + "AppServerConfig.xml")

                AppConfig = SysConfig
            End SyncLock

            Return AppConfig
        End Function

    End Class
End Namespace