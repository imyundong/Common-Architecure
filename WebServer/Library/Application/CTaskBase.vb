Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data

Public MustInherit Class CTaskBase
    Implements ITransaction
    Implements IComponent

    MustOverride Sub Process(DatabaseFactory As CDatabaseFactory)
    Public MustOverride ReadOnly Property Name As String Implements IComponent.Name
    Public Sub Debug() Implements ITransaction.Debug
        ' TODO
    End Sub
    Public ReadOnly Property TxnCode As String Implements ITransaction.TxnCode
        Get
            Return Nothing
        End Get
    End Property
End Class
