Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility


Namespace Utility
    Public Class CXmlConfig
        Public Function Load(Optional Path As String = "") As IConfiguration

            If Not Me.GetType.GetInterfaces.Contains(GetType(IConfiguration)) Then
                Return Nothing
            End If

            If String.IsNullOrEmpty(Path) Then
                Path = CUtility.ServerPath(DirectCast(Me, IConfiguration).Path)
            Else
                Path = CUtility.ServerPath(Path)
            End If

            Try
                Dim Config As IConfiguration
                If String.IsNullOrEmpty(Path) Then
                    Config = Activator.CreateInstance(Me.GetType)
                    Path = CUtility.ServerPath(Path)
                End If

                Using Sr As New IO.StreamReader(Path)
                    Dim XmlSerializer As New Xml.
                        Serialization.XmlSerializer(Me.GetType)
                    Config = XmlSerializer.Deserialize(Sr)
                End Using

                For Each Prop As System.Reflection.PropertyInfo In Me.GetType.GetProperties
                    If Prop.CanWrite Then
                        Dim Obj As Object = CallByName(Config, Prop.Name, CallType.Get)
                        CallByName(Me, Prop.Name, CallType.Set, Obj)
                    End If
                Next

                Config.OnSerialized()
                Dim This As IConfiguration = Me
                This.OnSerialized()

                Return Config

            Catch ex As Exception
                ' Log Error
                CLog.Err("Load File Error", ex)
                CLog.Err("File Path <{0}>", Path)
                Throw ex
            End Try

        End Function
    End Class
End Namespace