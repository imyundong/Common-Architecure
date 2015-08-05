Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports System.Xml.Serialization

Public Class CTxn999995
    Inherits TransactionBase

    Private Function RetrieveParent(ByVal MenuItem As CMenuInfo.CMenuItem, ByVal MenuItemId As Integer) As CMenuInfo.CMenuFolder
        If MenuItemId = MenuItem.MenuItemId Then
            Return MenuItem.MenuFolder
        End If

        For Each Item As CMenuInfo.CMenuItem In MenuItem.MenuFolder.MenuItem
            If Item.MenuFolder IsNot Nothing Then
                Dim Folder As CMenuInfo.CMenuFolder = RetrieveParent(Item, MenuItemId)
                If Folder IsNot Nothing Then
                    Return Folder
                End If
            End If
        Next

        Return Nothing
    End Function

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim Menu As New CMenuInfo
        Menu.Root = New CMenuInfo.CMenuFolder()

        Dim MenuItemRoot As New CMenuInfo.CMenuItem
        Dim MenuLinkRoot As New CMenuInfo.CMenuItem.CMenuItemLink
        MenuLinkRoot.Link = "#"
        MenuLinkRoot.Title = "MENU"
        MenuLinkRoot.Value = "MENU"
        MenuItemRoot.MenuItemId = 0
        MenuItemRoot.MenuFolder = New CMenuInfo.CMenuFolder

        MenuItemRoot.MenuLink = MenuLinkRoot
        Menu.Root.MenuItem.Add(MenuItemRoot)
        ' Keep All Validate Txn Path
        Dim TxnPaths As New Generic.Dictionary(Of Integer, CMenuInfo.CMenuFolder)
        TxnPaths.Add(0, Menu.Root)
        ' Retrieve All Menu Structure From Database
        Dim TxnPath As New CTxnPath
        Try
            Dim Idx As Integer = 0
            While (True)
                TxnPath.SearchOrderByPriority(DatabaseFactory, Idx, Nothing)
                Dim MenuItem As New CMenuInfo.CMenuItem
                MenuItem.MenuFolder = New CMenuInfo.CMenuFolder
                MenuItem.MenuLink = New CMenuInfo.CMenuItem.CMenuItemLink
                MenuItem.MenuItemId = TxnPath.TxnPathId

                With MenuItem.MenuLink
                    .Link = "#"
                    .More = ""
                    .Value = TxnPath.TxnPathDescription
                    .Title = TxnPath.TxnPathId.ToString + " : " + TxnPath.TxnPathDescription
                    If TxnPath.IconIndex > 0 Then
                        .Icon = New CMenuInfo.CMenuItem.CMenuIcon(TxnPath.IconIndex)
                    End If
                End With

                ' Append To Parent
                Dim ParentFolder As CMenuInfo.CMenuFolder = RetrieveParent(MenuItemRoot, TxnPath.Parent)
                If ParentFolder Is Nothing Then
                    CLog.Warning("Invalid Txn Path : {0} Of {1}:{2}", TxnPath.Parent, TxnPath.TxnPathId, TxnPath.TxnPathDescription)
                    Idx += 1
                    Continue While
                End If


                ' Append to Parent
                ParentFolder.MenuItem.Add(MenuItem)
                TxnPaths.Add(TxnPath.TxnPathId, MenuItem.MenuFolder)

                Idx += 1
            End While
        Catch ex As Exception

        End Try

        ' Retrieve User Transactions, Append to User Menu
        CLog.Info("Retrieve User Roles")
        Dim UserRoles As List(Of CUserRoleCategory) = GetUserRoles(DatabaseFactory, UserInfo.UserId)

        Dim Roles As New List(Of Integer)
        For Each Role As CUserRoleCategory In UserRoles
            CLog.Info("User Role {0} : {1}", Role.RoleId, Role.RoleDescription)
            Roles.Add(Role.RoleId)
        Next

        ' Retrieve Branch Infomation
        CLog.Info("Retrieve Branch : {0}", UserInfo.Branch)
        Dim Branch As New CBranch
        Branch.Search(DatabaseFactory, UserInfo.Branch, Nothing)
        Dim BranchCategory As Integer = Branch.BranchCategory
        CLog.Info("Branch Category : {0}", Branch.BranchCategory)

        ' Retrieve Business Group
        CLog.Info("Retrieve Business Group")
        Dim TxnPermission As New CTransactionPermission
        Dim BusinessGroups As New List(Of Integer)
        Try
            Dim Idx As Integer = 0
            While True
                TxnPermission.SearchByBranchAndTellerRole(DatabaseFactory, Branch.BranchCategory, Roles, Idx, Nothing)
                Idx += 1
                If Not BusinessGroups.Contains(TxnPermission.BusinessGroupId) Then
                    CLog.Info("Find Business Group : {0}", TxnPermission.BusinessGroupId)
                    BusinessGroups.Add(TxnPermission.BusinessGroupId)
                End If
            End While

        Catch ex As Exception

        End Try

        CLog.Info("Find User Transactions")
        Dim TransactionList As New HashSet(Of String)
        Dim BusinessGroup As New CBusinessGroup
        Try
            Dim Idx As Integer = 0
            While True
                BusinessGroup.SearchByBusinessGroupId(DatabaseFactory, BusinessGroups, Idx, Nothing)
                Idx += 1
                TransactionList.Add(BusinessGroup.TxnCode)
            End While

        Catch ex As Exception

        End Try

        ' Retrieve All Transactions
        CLog.Info("Totally {0} Transactions Found", TransactionList.Count)
        For Each Txn As String In TransactionList
            If Not TxnParameters.ContainsKey(Txn) Then
                CLog.Warning("Txn {0} in Business Group Is Not Defined in Transaction Paraemters", Txn)
                Continue For
            End If
            ' Get Transaction Information
            Dim TxnParameter As CTxnParameter = TxnParameters.Item(Txn)

            If Not TxnPaths.ContainsKey(TxnParameter.TxnPath) Then
                CLog.Warning("Txn {0} Path Is Not Available", TxnParameter.TxnPath)
                Continue For
            End If
            Dim MenuFolder As CMenuInfo.CMenuFolder = TxnPaths.Item(TxnParameter.TxnPath)
            Dim MenuItem As New CMenuInfo.CMenuItem
            MenuItem.MenuLink = New CMenuInfo.CMenuItem.CMenuItemLink
            With MenuItem.MenuLink
                .TxnCode = TxnParameter.TxnCode
                .Link = "javascript:$.Workflow.OpenTransaction('" + TxnParameter.TxnCode + "')"
                .Value = TxnParameter.TxnCode + " : " + TxnParameter.Description
                .Title = TxnParameter.Description
                If TxnParameter.TxnIcon > 0 Then
                    .Icon = New CMenuInfo.CMenuItem.CMenuIcon(TxnParameter.TxnIcon)
                End If
            End With

            TxnPaths.Item(TxnParameter.TxnPath).MenuItem.Add(MenuItem)
        Next

        ' Prepare User Menu
        Dim Serilizer As New XmlSerializer(GetType(CMenuInfo))
        Using Ms As New IO.MemoryStream()
            Serilizer.Serialize(Ms, Menu)

            Ms.Position = 0
            _Response.UserMenu = New IO.StreamReader(Ms).ReadToEnd
        End Using

    End Sub

    Private _Response As New CTxnResp999995
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999995"
        End Get
    End Property


    Public Class CMenuInfo
        <XmlElement("ul")>
        Property Root As CMenuFolder

        Public Class CMenuFolder

            <XmlElement("li")>
            Property MenuItem As New List(Of CMenuItem)

        End Class

        Public Class CMenuItem
            <XmlIgnore>
            Property MenuItemId As Integer = 0

            <XmlElement("a")>
            Property MenuLink As CMenuItemLink
            <XmlElement("ul")>
            Property MenuFolder As CMenuFolder

            Public Class CMenuItemLink
                <XmlAttribute("txn-code")>
                Property TxnCode As String
                <XmlAttribute("href")>
                Property Link As String
                <XmlAttribute("title")>
                Property Title As String
                <XmlElement("img")>
                Property Icon As CMenuIcon
                <XmlElement("span")>
                Property Value As String
                <XmlElement("b")>
                Property More As String


            End Class

            Public Class CMenuIcon
                Sub New()

                End Sub
                Sub New(Index As Integer)
                    Source = "Icons/Icon" + Index.ToString + ".png"
                End Sub

                <XmlAttribute("src")>
                Property Source As String

            End Class
        End Class

    End Class

    Public Class CTxnResp999995
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase

        Property UserMenu As String

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("UserMenu", UserMenu)

            Return Message
        End Function


    End Class
End Class

