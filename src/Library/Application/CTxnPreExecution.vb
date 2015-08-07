Public MustInherit Class CTxnPreExecution
    Inherits CTransactionBase
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return CTransactionBase.PRE_EXECUTED_TXN
        End Get
    End Property
End Class
