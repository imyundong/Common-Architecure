Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow.CError
Imports System.Security.Cryptography
Imports ServerPlatform.Library.Utility

''' <summary>
''' This Transaction Should be Done By SSO System And Return a Token to User.
''' Any Time User Wants to Access The Systemm, Token Should be Verified.
''' </summary>
''' <remarks></remarks>
Public Class CTxn009001
    Inherits BancslinkTxnBase

    Property AuthUser As String
    Property Password As String
    Property OverrideId As String
    Property LoginToken As String
    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("UserID", AuthUser)
        Message.GetValueByKey("Passo", Password)
        Message.GetValueByKey("OverrideId", OverrideId)
        Message.GetValueByKey("LoginToken", LoginToken)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim User As New CSystemUser

        Try
            ' Use Username & Password
            If String.IsNullOrEmpty(LoginToken) Then
                ' Retrieve User
                User.Search(DatabaseFactory, AuthUser, False)

                Dim Crypto As MD5 = MD5.Create
                Dim PwdHash As Byte() = Crypto.ComputeHash(Text.Encoding.ASCII.GetBytes(Password))

                If User.PinBlock <> CUtility.HexToString(PwdHash) Then
                    Throw New CBusinessException(CErrorCode.INVALID_USER_OR_PASSWORD, "Invalid User Id Or Password : {0}", AuthUser)
                End If
            Else
                If CToken.Tokens.ContainsKey(LoginToken) AndAlso
                   CToken.Tokens.Item(LoginToken).ExpiryDate > Now() Then
                    Dim Tk As CToken = CToken.Tokens.Item(LoginToken)
                    ' System Check
                    ' TODO
                    ' User Check
                    User.Search(DatabaseFactory, Tk.UserID, False)
                    CToken.Tokens.Remove(LoginToken)

                Else
                    Throw New CBusinessException(CErrorCode.USER_TOKEN_EXPIRED, "Invalid Login Token")
                End If
            End If

            Dim Token As String = Guid.NewGuid.ToString
            _Response.UserToken = Token
            ' User Validation
            If Not String.IsNullOrEmpty(UserId) Then
                ' Update Override Information
                Dim OverrideHistory As New COverrideHistory
                Try
                    Dim Idx As Integer = 0
                    While (True)
                        OverrideHistory.SearchByOverrideId(DatabaseFactory, OverrideId, Idx, True)
                        OverrideHistory.UpdateDate = Now
                        OverrideHistory.SupervisorId = AuthUser
                        OverrideHistory.Status = 1 ' Approved
                        OverrideHistory.Update(DatabaseFactory)
                        Idx += 1
                    End While
                Catch ex As Exception

                End Try

                CToken.Tokens.Add(Token, New CToken With {.ExpiryDate = Now.AddSeconds(CToken.SHORT_TERM_TOKEN), .Token = Token, .UserID = UserId})
                Exit Sub
            Else
                CToken.Tokens.Add(Token, New CToken With {.ExpiryDate = Now.Date.AddDays(1), .Token = Token, .UserID = AuthUser})
            End If

            ' Retrieve User Branch
            Dim Branch As New CBranch
            Branch.Search(DatabaseFactory, User.Branch, Nothing)

            _Response.Branch = User.Branch
            _Response.BranchName = Branch.BranchName
            _Response.Username = User.FullName
            _Response.ContactNo = User.ContactNo
            _Response.IPAddress = IPAddress
            _Response.UserId = User.UserId

            ' Retrieve User Role
            _Response.UserRole = GetUserRoles(DatabaseFactory, AuthUser)
        Catch ex As CBusinessException
            If ex.ErrCode = CErrorCode.RECORD_NOT_FOUND Then
                Throw New CBusinessException(Library.Workflow.CError.CErrorCode.INVALID_USER_OR_PASSWORD, "Invalid User Id Or Password")
            Else
                Throw ex
            End If
        End Try
    End Sub

    Private _Response As New CTxnResp009001
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "009001"
        End Get
    End Property

    Public Class CTxnResp009001
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase
        Property UserId As String
        Property IPAddress As String
        Property Branch As Integer
        Property UserRole As New List(Of CUserRoleCategory)
        Property Username As String
        Property BranchName As String
        Property ContactNo As String
        Property LastLoginDate As Date = Now
        Property FailAttempts As Integer = 3
        Property Nationality As String = "CN"
        Property UserToken As String = ""

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()

            Message.IPAddress = IPAddress
            Message.SetValue("Branch", Branch)
            Message.SetValue("UserRole", UserRole)
            Message.SetValue("Username", Username)
            Message.SetValue("BranchName", BranchName)
            Message.SetValue("Nationality", Nationality)
            Message.SetValue("ContactNo", ContactNo)
            Message.SetValue("AuthorizedUser", UserId)
            Message.SetValue("LastLoginDate", LastLoginDate)
            Message.SetValue("FailAttempts", FailAttempts)
            Message.SetValue("UserToken", UserToken)
            Message.SetValue("UserId", UserId)

            Return Message
        End Function


    End Class
End Class

