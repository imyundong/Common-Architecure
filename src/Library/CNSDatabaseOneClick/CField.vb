Public Class CField
    ' Id
    Public Property Id As String = ""
    ' DataType
    Private _DataType As String = ""

    Public Property DataType As String
        Get
            Return _DataType
        End Get
        Set(value As String)
            If SupportDataType.Contains(value) Then
                _DataType = value
            Else
                _DataType = CEnviorment.INVALID
            End If
        End Set
    End Property

    Public Property Length As String = 0

    Public Property Key As Boolean = False

    Public Property NotNull As Boolean = False

    Public Property Value As String = ""

    Public Shared ReadOnly SupportDataType As List(Of String) = {"VARCHAR", "CHAR", "DATETIME", "INT", "FLOAT", "IDENTITY"}.ToList()

    Public Function GetInternalDataType() As String
        Select Case DataType
            Case "VARCHAR"
                Return "String"
            Case "CHAR"
                If Length = 1 Then
                    Return "Char"
                Else
                    Return "Char(" & Length & ")"
                End If
            Case "DATETIME"
                Return "DateTime"
            Case "INT"
                If Length = 2 Then
                    Return "Short"
                ElseIf Length = 4 Then
                    Return "Integer"
                ElseIf Length = 8 Then
                    Return "Long"
                End If
            Case "FLOAT"
                If Length = 4 Then
                    Return "Single"

                ElseIf Length = 8 Then
                    Return "Double"
                End If
            Case "IDENTITY"
                Return "Integer"
        End Select


        Return "String"
    End Function

End Class
