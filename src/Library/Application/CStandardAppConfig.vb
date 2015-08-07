Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility

<Xml.Serialization.XmlRoot("AppConfig")>
Public Class CStandardAppConfig
    Inherits ServerPlatform.Library.Data.CApplicationConfig
    Property Request As CCommInfo.CHostInfo

    <Xml.Serialization.XmlArray("Hosts")>
    <Xml.Serialization.XmlArrayItem("Server")>
    Property Hosts As New List(Of CCommInfo.CHostInfo)

    Property Database As CDatabaseInfo

    <Xml.Serialization.XmlArrayItem("Txn")>
    Property TxnMapping As New List(Of CTxnMapping)
    <Xml.Serialization.XmlIgnore>
    Property TxnMappingDictionary As New Generic.Dictionary(Of String, String)
    Public Class CTxnMapping
        <Xml.Serialization.XmlAttribute>
        Property TxnCode As String
        <Xml.Serialization.XmlAttribute>
        Property MapTo As String
    End Class

End Class
