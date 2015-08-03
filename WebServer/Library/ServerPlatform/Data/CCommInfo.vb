Imports System.Xml.Serialization
Namespace Data
    Public Class CCommInfo
        Property ComponentPath As String = "Communication"
        <XmlElement>
        Property Host As New List(Of CHostInfo)

        Public Class CHostInfo
            <XmlAttribute>
            Property Id As String = ""
            Property Component As New CCommComponentInfo
            Property Converter As CConverterInfo
        End Class

        Public Class CConverterInfo
            <XmlAttribute>
            Property Category As String
            Property WorkingPath As String = ""
            Property Master As String

            Property Encoding As String
        End Class

        Public Class CCommComponentInfo
            <XmlAttribute>
            Property Assembly As String
            <XmlAttribute>
            Property Category As String

            <XmlAttribute>
            Property MaxConnections As Integer = 500

            <XmlAttribute>
            Property Mode As CMode = CMode.Async
            <XmlAttribute>
            Property RunAt As CRunAt = CRunAt.Client
            <XmlAttribute>
            Property Timeout As Integer = 30000
            Property Address As String = ""
            Property Port As Integer = 0
            ' For Async Use Only
            Property LocalPort As Integer = 0
            Property MessageLen As Integer = 5
            Property MessageLenType As CMessageLenType = CMessageLenType.ASCII

            Property MessageLenIncluded As Boolean = False

            Public Enum CMessageLenType As Integer
                Hex = 0
                ASCII = 1
            End Enum
            Public Enum CMode As Integer
                Async = 0
                Sync = 1
            End Enum

            Public Enum CRunAt As Integer
                Client = 0
                Server = 1
            End Enum
        End Class
    End Class

End Namespace