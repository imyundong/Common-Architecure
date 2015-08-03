Imports ServerPlatform.Library.Data
Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility
Public Class CTxnPreExecuted
    Inherits CTxnPreExecution

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        ' TODO
    End Sub

    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return Nothing
        End Get
    End Property
End Class

