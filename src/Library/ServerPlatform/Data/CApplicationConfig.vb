Namespace Data
    <Xml.Serialization.XmlRoot("AppConfig")>
    <Serializable>
    Public Class CApplicationConfig
        Inherits Utility.CXmlConfig
        Implements Utility.IConfiguration
        ' Keep Internal Instance
        Private Shared AppConfig As CApplicationConfig
        <Xml.Serialization.XmlElement("Log")>
        Property LogInfo As New CLogInfo
        <Xml.Serialization.XmlIgnore>
        Property TxnList As New Generic.Dictionary(Of String, Type)
        <Xml.Serialization.XmlIgnore>
        Property ComponentList As New Generic.Dictionary(Of String, Type)
        <Xml.Serialization.XmlIgnore>
        Property DatabaseAccessList As New Generic.Dictionary(Of String, Type)

        Public Overridable Sub OnSerialized() Implements Utility.IConfiguration.OnSerialized
            AppConfig = Me
        End Sub

        Public ReadOnly Property Path As String Implements Utility.IConfiguration.Path
            Get
                Return ""
            End Get
        End Property

        Public Shared Function GetInstance()
            Return AppConfig
        End Function
    End Class
End Namespace