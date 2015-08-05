Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow.CError
Imports System.Security.Cryptography
Imports ServerPlatform.Library.Utility

''' <summary>
''' This Transaction Should be Done By SSO System And Return a Token to User.
''' Any Time User Wants to Access The Systemm, Token Should be Verified.
''' </summary>
''' <remarks></remarks>
Public Class CTxn009600
    Inherits BancslinkTxnBase

    Property AuthUser As String
    Property Password As String
    Public Overrides Sub Decode(Message As Library.Data.CMessage)
        MyBase.Decode(Message)
        Message.GetValueByKey("UserID", AuthUser)
        Message.GetValueByKey("Passo", Password)
    End Sub

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim User As New CSystemUser
        Dim Crypto As MD5 = MD5.Create
        Dim PwdHash As Byte() = Crypto.ComputeHash(Text.Encoding.ASCII.GetBytes(Password))

        Try
            ' Retrieve User
            User.Search(DatabaseFactory, AuthUser, False)
            ' Retrieve User Branch
            Dim Branch As New CBranch
            Branch.Search(DatabaseFactory, User.Branch, Nothing)

            _Response.Branch = User.Branch
            _Response.BranchName = Branch.BranchName
            _Response.Username = User.FullName
            _Response.ContactNo = User.ContactNo
            _Response.IPAddress = IPAddress
            _Response.UserId = AuthUser

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
            Return "009600"
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

            Return Message
        End Function


    End Class
End Class

