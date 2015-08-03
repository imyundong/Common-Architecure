Public Class CToken
    Public Shared Tokens As New Generic.Dictionary(Of String, CToken)
    Property Token As String
    Property TokenType As CTokenType = CTokenType.UserToken

    Public Enum CTokenType As Integer
        UserToken = 0
        LoginToken = 1
    End Enum
    Property ExpiryDate As Date
    Property UserID As String
    Property LinkedToken As CToken

    Public Const SHORT_TERM_TOKEN As Integer = 60
End Class
