Namespace Utility
    Public Class CReadWriteLocker
        Private Sema As Threading.Semaphore
        Private Mutex As New Threading.Mutex(False, "Semaphore_Mutex")

        Public Sub New(ByVal Name As String)
            Try
                Sema = Threading.Semaphore.OpenExisting(Name & "_Semaphore")
            Catch ex As Exception
                Sema = New Threading.Semaphore(100, 100, Name & "_Semaphore")
            End Try
            
        End Sub

        Public Sub AcquireReaderLock()
            Sema.WaitOne()
        End Sub

        Public Sub ReleaseReaderLock()
            Sema.Release()
        End Sub

        Public Sub AcquireWriterLock()
            Mutex.WaitOne()
            For i As Integer = 0 To 99
                Sema.WaitOne()
            Next
            Mutex.ReleaseMutex()
        End Sub

        Public Sub ReleaseWriterLock()
            For i As Integer = 0 To 99
                Sema.Release()
            Next
        End Sub
    End Class

End Namespace

