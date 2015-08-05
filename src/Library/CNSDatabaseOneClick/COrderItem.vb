Public Class COrderItem
    Public Property Field As CField
    ' Order By
    Private _Order As String = "ASC"
    Public Property Order As String
        Get
            Return _Order
        End Get
        Set(value As String)
            If SupportOrder.Contains(value) Then
                _Order = value
            Else
                _Order = CEnviorment.INVALID
            End If
        End Set
    End Property
    Private SupportOrder As List(Of String) = {"ASC", "DESC"}.ToList
End Class
