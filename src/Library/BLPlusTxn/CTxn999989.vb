Imports ServerPlatform.Library.Data

Public Class CTxn999989
    Inherits TransactionBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim UserToken As CToken = Nothing
        ' Hard Code ' TODO ' BUG
        For Each Token As CToken In CToken.Tokens.Values
            If Token.UserID = UserId Then
                UserToken = Token
            End If
        Next

        If UserToken Is Nothing Then
            ' Generate New Token
            UserToken = New CToken With {.Token = Guid.NewGuid.ToString,
                .ExpiryDate = Now.Date.AddDays(1),
                .UserID = UserId}

            CToken.Tokens.Add(UserToken.ToString, UserToken)
        End If

        Dim Token1 As New CToken() With {.Token = Guid.NewGuid.ToString,
            .ExpiryDate = Now.AddSeconds(CToken.SHORT_TERM_TOKEN),
            .UserID = UserId,
            .LinkedToken = UserToken,
            .TokenType = CToken.CTokenType.LoginToken}

        CToken.Tokens.Add(Token1.Token, Token1)
        _Response.LoginToken = Token1.Token

    End Sub

    Private _Response As New CTxnResp999989
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999989"
        End Get
    End Property

    Public Class CTxnResp999989
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property LoginToken As String
        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("LoginToken", LoginToken)

            Return Message
        End Function

    End Class
End Class

