Public Class CKeyItem
    Public Property Field As CField
    Property Repeat As Integer = 1

    Private _Oper As String = "="
    Public Property Oper As String
        Get
            Return _Oper
        End Get
        Set(value As String)
            If SupportOperator.Contains(value) Then
                _Oper = value
            Else
                _Oper = CEnviorment.INVALID
            End If
        End Set
    End Property

    Private _Relation As String = "AND"
    Public Property Relation As String
        Get
            Return _Relation
        End Get
        Set(value As String)
            If SupportRelation.Contains(value) Then
                _Relation = value
            Else
                _Relation = CEnviorment.INVALID
            End If
        End Set
    End Property

    Private SupportRelation As List(Of String) = {"AND", "OR"}.ToList

    Private SupportOperator As List(Of String) = {"=", ">", ">=", "<", "<=", "<>", "LIKE", "IN”, "NOT IN"}.ToList

    ' Supported Method 
    ' TODATETIME, TODATE
    Public Property Method As String = ""
    Public Property InternalMethod As String = ""

    Public Shared SupportMethod As List(Of String) = {"TODATETIME", "TODATE"}.ToList

End Class
