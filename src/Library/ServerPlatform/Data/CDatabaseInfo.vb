
Namespace Data
    <Serializable>
    Public Class CDatabaseInfo
        <Xml.Serialization.XmlAttribute>
        Property DBType As CDBType = CDBType.SQLServer

        <Xml.Serialization.XmlAttribute>
        Property SystemAuth As Boolean = False
        Property UserId As String
        Property Password As String
        Property Database As String
        Property Source As String

        <Xml.Serialization.XmlAttribute>
        Property ConnectionString As String = ""

        Public Enum CDBType As Integer
            SQLServer = 0
            Access = 1
            Oracle = 2
            MySQL = 3
        End Enum
    End Class
End Namespace