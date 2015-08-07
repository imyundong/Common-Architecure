Imports ServerPlatform.Library.Data
Namespace Utility
    Public Interface IDatabaseAdapter
        Property Info As CDatabaseInfo
        Sub Open()
        Sub Close()

        Sub Commit()
        Sub Rollback()
        Sub AddWithValue(ByVal Field As String, Value As Object)

        ReadOnly Property Command As IDbCommand

        ReadOnly Property Adapter As IDbDataAdapter

        Property Status As CStatus

        Enum CStatus As Integer
            NotInitialised = 0
            Available = 1
            Failed = 2
        End Enum
    End Interface
End Namespace