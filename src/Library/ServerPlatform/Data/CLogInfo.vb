Imports System.Xml.Serialization
Namespace Data
    <Serializable>
    Public Class CLogInfo
        ' Log Size
        Public Property LogSize As Integer = 4 * 1024 * 1024
        ' Log Prefix
        Public Property Prefix As String = "Application"
        ' Log Count
        Public Property LogCount As Integer = 99
        ' Log Level
        Public Property LogLevel As String = "Info, Debug, Warning, Error, Txn, System"
        ' Log Path
        Public Property LogPath As String = "Logs"

        Private _LogLevelCollection As String()
        <XmlIgnore>
        Public ReadOnly Property LogLevelCollection As String()
            Get
                Return _LogLevelCollection
            End Get
        End Property


        Public Sub Init()
            _LogLevelCollection = LogLevel.Split(","c)

            For Idx As Integer = 0 To _LogLevelCollection.Count - 1
                _LogLevelCollection(Idx) = _LogLevelCollection(Idx).Trim.ToUpper
            Next
        End Sub

    End Class
End Namespace