Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Namespace Utility

    Public Class CDatabaseFactory
        Private Shared AdapterType As Type
        Private _DatabaseAdapter As IDatabaseAdapter
        Public ReadOnly Property DatabaseAdapter As IDatabaseAdapter
            Get
                Return _DatabaseAdapter
            End Get
        End Property

        Private Info As CDatabaseInfo
        Public Function CreateInstance() As IDatabaseAdapter
            If _DatabaseAdapter Is Nothing Then
                _DatabaseAdapter = Activator.CreateInstance(AdapterType)
                Try
                    _DatabaseAdapter.Info = Info
                    _DatabaseAdapter.Open()

                    If _DatabaseAdapter.Status <> IDatabaseAdapter.CStatus.Available Then
                        Throw New CError.CBusinessException(CError.CErrorCode.DB_CONNECTION_FAIL, "Database Connection Fail")
                    End If
                Catch ex As Exception
                    Throw ex
                    CLog.Err("Database Connection Fail, {0}", ex.Message)
                End Try
            End If

            Return _DatabaseAdapter
        End Function

        Public Sub New(ByVal Info As CDatabaseInfo)
            If Info Is Nothing Then
                Throw New CError.CBusinessException(CError.CErrorCode.DB_CONNECTION_FAIL, "Invalid Database Information")
            End If

            Me.Info = Info

            If AdapterType Is Nothing Then
                ' Get Application Config
                Dim AppConfig As CApplicationConfig = CApplicationConfig.GetInstance()
                CLog.Info("Retrieve Available Database Component for {0}", Info.DBType)

                For Each Item In AppConfig.ComponentList
                    ' Get Short Name
                    Dim CompoonentName() As String = Item.Key.Split("."c)
                    Dim ShortName As String = CompoonentName(CompoonentName.Length - 1)

                    ' Retrive Available Database From Component Cache
                    If ShortName = Info.DBType.ToString.ToUpper AndAlso _
                        Item.Value.GetInterfaces.Contains(GetType(IDatabaseAdapter)) Then
                        CLog.Info("Found Database Adapter {{{0}}}", Item.Value.GetType.FullName)
                        AdapterType = Item.Value
                    End If
                Next
            End If

            If AdapterType Is Nothing Then
                CLog.Err("Invalid Database Category Or Database Adapter is Not Available.")
                Throw New CError.CBusinessException(CError.CErrorCode.DB_CONNECTION_FAIL, "Invalid Database Type {0}", Info.DBType)
            End If
        End Sub

        Public Function GetDatabaseAccessLibrary(ByVal Name As String) As IDatabaseAccess
            Dim AppConfig As CApplicationConfig = CApplicationConfig.GetInstance()
            Dim ClassName As String = Info.DBType.ToString + "_" + Name
            CLog.Debug("Reflection", ClassName)
            If AppConfig.DatabaseAccessList.ContainsKey(ClassName.ToUpper) Then
                Return Activator.CreateInstance(AppConfig.DatabaseAccessList.Item(ClassName.ToUpper))
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_DATABASE_CLASS, "Database Class  {{{0}}} Does Not Exist", ClassName)
            End If
        End Function
    End Class
End Namespace