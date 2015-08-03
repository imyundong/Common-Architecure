Imports ServerPlatform.Library.Data
Namespace Utility
    Public Interface ITransaction
        ReadOnly Property TxnCode As String
        Sub Debug()
    End Interface
End Namespace