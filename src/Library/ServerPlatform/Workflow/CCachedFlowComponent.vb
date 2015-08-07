Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Namespace Workflow
    Public MustInherit Class CCachedFlowComponent
        Inherits CWorkflowComponent

        ' Must Override Name
        Public MustOverride Overrides ReadOnly Property Name As String

        ' Cache Component
        Private CachedComponentList As New Generic.Dictionary(Of String, CCachedFlowComponent)

        Private SyncObject As New Object

        ' Function Can not be Rewrite in the Sub-Class
        Public NotOverridable Overrides Sub Process()
            If String.IsNullOrEmpty(Key) Then
                CLog.Info("Key Is Nothing, Skip Cache")
                CacheProcess()
            End If

            Dim FullKey As String = Me.GetType.ToString + "_" + Key

            CLog.Info("Check Component In Cache {{{0}}}", FullKey)
            If CachedComponentList.ContainsKey(FullKey) Then
                ' Cached Component
                Dim CachedComponent As CCachedFlowComponent = CachedComponentList.Item(FullKey)

                For Each PropertyInfo As System.Reflection.PropertyInfo In Me.GetType.GetProperties()
                    If PropertyInfo.CanRead And PropertyInfo.CanWrite Then
                        CallByName(Me, PropertyInfo.Name, CallType.Set, CachedComponent)
                    End If
                Next
            Else
                SyncLock Me
                    Try
                        CacheProcess()
                        CLog.Sys("Transaction Cached")
                        ' Cache Current Component
                        CachedComponentList.Add(FullKey, Me)
                    Catch ex As Exception
                        Throw New CError.CBusinessException(CError.CErrorCode.INTERNAL_ERROR, ex)
                    End Try
                End SyncLock
            End If
        End Sub

        Public MustOverride Sub CacheProcess()

        ' Create a Key to Cache the Component
        Public MustOverride ReadOnly Property Key As String
    End Class
End Namespace