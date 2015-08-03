Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow.CError
Imports System.Security.Cryptography
Imports ServerPlatform.Library.Utility

''' <summary>
''' Validate User Token in SSO Server
''' Update Override Information
''' </summary>
''' <remarks></remarks>
Public Class CTxn009002
    Inherits BancslinkTxnBase

    Property OverrideId As String
    Property TokenId As String

    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)

        Message.GetValueByKey("OverrideId", OverrideId)
        Message.GetValueByKey("TokenId", TokenId)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        ' Should Connect to SSO Server
        ' TODO
        If CToken.Tokens.ContainsKey(TokenId) Then
            Dim Token As CToken = CToken.Tokens.Item(TokenId)
            If Token.ExpiryDate < Now() Then
                CToken.Tokens.Remove(TokenId)
                Throw New CBusinessException(CErrorCode.USER_TOKEN_EXPIRED, "The Token Expired")
            End If

            If Not String.IsNullOrEmpty(OverrideId) Then
                ' Update Override Information
                Dim OverrideHistory As New COverrideHistory

                Try
                    Dim Idx As Integer = 0
                    While (True)
                        OverrideHistory.SearchByOverrideId(DatabaseFactory, OverrideId, Idx, True)
                        OverrideHistory.UpdateDate = Now
                        OverrideHistory.SupervisorId = Token.UserID
                        OverrideHistory.Status = 1 ' Approved
                        OverrideHistory.Update(DatabaseFactory)
                        Idx += 1
                    End While
                Catch ex As Exception

                End Try

            End If

            _Response.UserId = Token.UserID
            _Response.UserToken = TokenId
        Else
            Throw New CBusinessException(CErrorCode.INVALID_USER_TOKEN, "Invalid User Token")
        End If
    End Sub

    Private _Response As New CTxnResp009002
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "009002"
        End Get
    End Property

    Public Class CTxnResp009002
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property UserId As String
        Property UserToken As String = ""

        Public Overrides Function Encode() As CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("UserId", UserId)
            Message.SetValue("UserToken", UserToken)

            Return Message
        End Function
    End Class
End Class

