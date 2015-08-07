Imports ServerPlatform.Library.Workflow
Namespace Data
    ''' <summary>
    ''' Application Static Summary
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CAppStatus
        Inherits MarshalByRefObject

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function
        ' Number Of Transaction
        Public TxnCount As Long
        Public TotalTxnCount As Long
        ' Average Process Time
        ReadOnly Property AverageProcTime As Double
            Get
                Return TotalProcTime / (TotalTxnCount * 1000)
            End Get
        End Property

        Public TotalProcTime As Long
        ' Application Status
        Public Status As CRuntimeStatus = CRuntimeStatus.Stopped
        ' Application Error
        Public ErrCode As CError.CErrorCode = CError.CErrorCode.SUCCESSFUL

        Sub IncreaseTxnCount()
            Threading.Interlocked.Increment(TxnCount)
            Threading.Interlocked.Increment(TotalTxnCount)
        End Sub

        Sub DecreaseTxnCount(ProcTime As Long)
            Threading.Interlocked.Decrement(TxnCount)
            Threading.Interlocked.Add(TotalProcTime, ProcTime)
        End Sub

        Public Enum CRuntimeStatus As Integer
            Stopped = 0
            Starting = 1
            Started = 2
            Stopping = 3
            Paused = 4
            Failed = 9
        End Enum
    End Class
End Namespace
