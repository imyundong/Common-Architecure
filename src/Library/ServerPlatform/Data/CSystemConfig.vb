Imports System.Xml.Serialization
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow


Namespace Data
    <Xml.Serialization.XmlRoot("SystemConfig")>
    <Serializable>
    Public Class CSystemConfig
        Inherits CXmlConfig
        Implements IConfiguration
#Region "Properties"
        <Xml.Serialization.XmlElement("Log")>
        Property LogInfo As New CLogInfo
        Property Communication As New CCommInfo
        Property Mode As String = "Integrated"
        <XmlIgnore>
        Property Com As ICommunication
        Property Workflow As New CWorkflowConfig
        Property SSOSetting As New CSSOSetting

#End Region

        Public Sub OnSerialized() Implements IConfiguration.OnSerialized

        End Sub

        Public ReadOnly Property Path As String Implements IConfiguration.Path
            Get
                Return "Config/SystemConfig.xml"
            End Get
        End Property

        Private Shared SystemConfig As CSystemConfig
        Private Shared SyncObject As New Object
        Public Shared Function GetInstance() As CSystemConfig
            If Not SystemConfig Is Nothing Then
                Return SystemConfig
            End If

            SyncLock SyncObject
                If Not SystemConfig Is Nothing Then
                    Return SystemConfig
                End If

                Dim SysConfig As New CSystemConfig
                SysConfig.Load("Config" + IO.Path.DirectorySeparatorChar + "SystemConfig.xml")

                SystemConfig = SysConfig
            End SyncLock

            Return SystemConfig
        End Function

    End Class

    Public Class CSSOSetting
        Property Enable As Boolean
        Property Url As String
        Property UserAuthPage As String
        Property LoginTokenPage As String
    End Class

    Public Class CWorkflowConfig
        Property Path As String = "Workflows"
        Property ComponentPath As String = "Bin"
    End Class

End Namespace