Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow

Public Class CSQLServerConnection
    Implements IComponent
    Implements IDatabaseAdapter
    Implements IDisposable


    Private Txn As SqlClient.SqlTransaction
    Private Connection As SqlClient.SqlConnection
    Private Parameter As SqlClient.SqlParameter
    Private Cmd As SqlClient.SqlCommand

    Private Shared ConnStr As String

    Public Sub Open() Implements IDatabaseAdapter.Open
        If String.IsNullOrEmpty(ConnStr) Then
            If Info Is Nothing Then
                Throw New CError.CBusinessException(CError.CErrorCode.DB_CONNECTION_FAIL, "Invalid Database Information")
            End If

            If Info.DBType <> CDatabaseInfo.CDBType.SQLServer Then _
                Throw New CError.CBusinessException(CError.CErrorCode.DB_CONNECTION_FAIL, "Invalid Database Category {0}", Info.DBType)

            Dim ConnectionString As New Text.StringBuilder
            ' Append Database Source/Name
            If Not String.IsNullOrEmpty(Info.Source) Then ConnectionString.Append("Data Source=" + Info.Source + "; ")
            If Not String.IsNullOrEmpty(Info.Database) Then ConnectionString.Append("Initial Catalog=" + Info.Source + "; ")

            ' System Autentication
            If Info.SystemAuth = True Then
                If Not String.IsNullOrEmpty(Info.Database) Then ConnectionString.Append("Integrated Security=true; ")
            Else
                If Not String.IsNullOrEmpty(Info.UserId) Then ConnectionString.Append("User ID=" + Info.UserId + "; ")
                If Not String.IsNullOrEmpty(Info.Password) Then ConnectionString.Append("Password=" + Info.Password + "; ")
            End If

            If Not String.IsNullOrEmpty(Info.ConnectionString) Then ConnectionString.Append(Info.ConnectionString)
            ConnStr = ConnectionString.ToString

            CLog.Debug("Connection String : {0}", ConnStr)
        End If

        ' Connect to Database
        Try
            Connection = New SqlClient.SqlConnection(ConnStr)
            Connection.Open()

            If Connection.State = ConnectionState.Open Then
                CLog.Info("Connection Stauts Open")
                CLog.Sys("Beign Transaction")
                Txn = Connection.BeginTransaction
            Else
                CLog.Err("Invalid Conenction Status : {0}", Connection.State)
                Status = IDatabaseAdapter.CStatus.Failed
                Exit Sub
            End If
        Catch ex As Exception
            CLog.Err("Connection Failed", ex)
            If Not Connection Is Nothing Then
                Connection.Close()
            End If

            Status = IDatabaseAdapter.CStatus.Failed
            Exit Sub
        End Try

        Status = IDatabaseAdapter.CStatus.Available
    End Sub

    Public ReadOnly Property Name As String Implements IComponent.Name
        Get
            Return "SQLServer"
        End Get
    End Property

    Public ReadOnly Property Adapter As IDbDataAdapter Implements IDatabaseAdapter.Adapter
        Get
            Return New SqlClient.SqlDataAdapter()
        End Get
    End Property

    Public Sub Close() Implements IDatabaseAdapter.Close
        Try
            If Not Connection Is Nothing Then Connection.Close()
        Catch ex As Exception
            CLog.Warning("Closing Connection Error : {0}", ex.Message)
        End Try
    End Sub

    Public ReadOnly Property Command As IDbCommand Implements IDatabaseAdapter.Command
        Get
            Cmd = New SqlClient.SqlCommand
            Cmd.Connection = Connection
            Cmd.Transaction = Txn

            Return Cmd
        End Get
    End Property

    Public Property Info As CDatabaseInfo Implements IDatabaseAdapter.Info

    Public Property Status As IDatabaseAdapter.CStatus = IDatabaseAdapter.CStatus.NotInitialised Implements IDatabaseAdapter.Status

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO:  释放托管状态(托管对象)。
                If Not Connection Is Nothing Then
                    Connection.Close()
                    Connection = Nothing
                End If
            End If

            ' TODO:  释放非托管资源(非托管对象)并重写下面的 Finalize()。
            ' TODO:  将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO:  仅当上面的 Dispose(ByVal disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 不要更改此代码。    请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。    请将清理代码放入上面的 Dispose (disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Public Sub Commit() Implements IDatabaseAdapter.Commit
        Try
            If Not Connection Is Nothing And Not Txn Is Nothing Then
                Txn.Commit()
            End If
        Catch ex As Exception
            CLog.Warning("Error in Commit : {0}", ex.Message)
        End Try

    End Sub

    Public Sub Rollback() Implements IDatabaseAdapter.Rollback
        Try
            If Not Connection Is Nothing And Not Txn Is Nothing Then
                Txn.Rollback()
            End If
        Catch ex As Exception
            CLog.Warning("Error in Rollback : {0}", ex.Message)
        End Try
    End Sub

    Public Sub AddWithValue(Field As String, Value As Object) Implements IDatabaseAdapter.AddWithValue
        Cmd.Parameters.AddWithValue(Field, Value)
    End Sub
End Class
