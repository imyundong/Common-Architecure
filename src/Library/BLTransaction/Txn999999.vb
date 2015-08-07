Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Workflow
Imports System.Xml.Serialization
Imports ServerPlatform.Application

Public Class Txn999999
    Inherits BancslinkTxnBase

    ' Find Menu in List
    Private Function FindMenuItemByParentId(ByVal MenuItems As List(Of CMenuRoot.CMenuItem), _
                                            ByVal ParentId As Integer) As List(Of CMenuRoot.CMenuItem)
        Dim MenuItemList As New List(Of CMenuRoot.CMenuItem)

        For Each Item As CMenuRoot.CMenuItem In MenuItems
            If Item.Parent = ParentId Then
                MenuItemList.Add(Item)
            End If
        Next

        Return MenuItemList
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="MenuItem">Current Menu Item</param>
    ''' <remarks></remarks>
    Private Sub CreateMenuFolder(ByVal MenuItem As CMenuRoot.CMenuItem)
        Dim MenuItems As List(Of CMenuRoot.CMenuItem) = FindMenuItemByParentId(MenuFolders.Values.ToList, MenuItem.Id)
        For Each Item As CMenuRoot.CMenuItem In MenuItems
            MenuItem.MenuItems.Add(Item)
            CreateMenuFolder(Item)
        Next
    End Sub

    Private MenuFolders As New Generic.Dictionary(Of String, CMenuRoot.CMenuItem)
    Private Shared TxnPathList As New List(Of CTxnPath)

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        CLog.Info("This Txn 999999")
        CLog.Info("Creating Menu")
        If TxnPathList.Count = 0 Then
            Dim Index As Integer = 0
            Dim TxnPath As New CTxnPath
            While True
                Try

                    TxnPath.SearchByOrder(DatabaseFactory, Index, False)
                    TxnPathList.Add(TxnPath.Clone)

                    Index += 1
                Catch ex As Exception
                    Exit While
                End Try
            End While
        End If

        CLog.Info("Totally {0} Txn Path Found", TxnPathList.Count)
        For Each MyTxnPath As CTxnPath In TxnPathList
            Dim MenuItem As New CMenuRoot.CMenuItem(MyTxnPath.PathName, MyTxnPath.TxnPathId, MyTxnPath.ParentId)
            MenuFolders.Add(MenuItem.Id, MenuItem)
        Next

        If MenuFolders.Count = 0 Then
            Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Menu Folder Not Found")
        End If

        ' Create Menu Root
        Dim MenuInfo As New CMenuRoot
        MenuInfo.MenuItems = New List(Of CMenuRoot.CMenuItem)

        Dim CurrentMenuItems As List(Of CMenuRoot.CMenuItem) = MenuInfo.MenuItems
        ' Create Infrastructure
        Dim RootFolder As List(Of CMenuRoot.CMenuItem) = FindMenuItemByParentId(MenuFolders.Values.ToList, 0)
        For Each Item As CMenuRoot.CMenuItem In RootFolder
            MenuInfo.MenuItems.Add(Item)
            CreateMenuFolder(Item)
        Next

        ' Retrieve Branch Category
        Dim BranchCategory As New CBranch
        BranchCategory.Search(DatabaseFactory, BranchId, False)

        ' Prepare Transactions
        CLog.Info("This Txn 999999")
        CLog.Info("Creating Menu")

        Dim Cmd As New Text.StringBuilder("SELECT Distinct TxnCode, TxnPath, Transactions.Title, PageId, FlowId, NodeId")
        Cmd.Append(" FROM Teller, TellerRole, TransactionPermission, BussinessGroup, Transactions")
        Cmd.Append(" WHERE Teller.Tellerid = @TELLERID And")
        Cmd.Append(" Teller.Tellerid = TellerRole.Tellerid And")
        Cmd.Append(" TransactionPermission.TellerRoleId = TellerRole.TellerRoleId And")
        Cmd.Append(" TransactionPermission.BranchCategoryId = " + BranchCategory.BranchCategoryId.ToString + " And")
        Cmd.Append(" BussinessGroup.BizGroupId = TransactionPermission.BusinessGroup And")
        Cmd.Append(" Transactions.TxnNo = BussinessGroup.TxnCode")

        Dim Adapter As IDatabaseAdapter = DatabaseFactory.CreateInstance()
        Dim Command As IDbCommand = Adapter.Command
        Command.CommandText = Cmd.ToString
        Adapter.AddWithValue("TELLERID", TellerInfo.TellerId)
        Dim DataAdapter As IDbDataAdapter = Adapter.Adapter
        DataAdapter.SelectCommand = Command

        Dim Ds As New DataSet
        DataAdapter.Fill(Ds)

        CLog.Info("{0} Transactions Found", Ds.Tables(0).Rows.Count)
        For Each Record As DataRow In Ds.Tables(0).Rows
            If MenuFolders.ContainsKey(Record.Item(1)) Then
                MenuFolders.Item(Record.Item(1)).MenuItems.Add(New CMenuRoot.CMenuItem(Record.Item(0), Record.Item(3), Record.Item(2), Record.Item(4), Record.Item(5)))
            Else
                CLog.Warning("Transaction {0}'s Folder {1} is Not Defined", Record.Item(0), Record.Item(1))
            End If
        Next

        Dim Content As String = ""
        Using Ms As New IO.MemoryStream
            Dim Serilizer As New Xml.Serialization.XmlSerializer(GetType(CMenuRoot))
            Serilizer.Serialize(Ms, MenuInfo)
            Content = System.Text.Encoding.UTF8.GetString(Ms.ToArray())
        End Using

        TxnResp99999.MenuContent = Content
    End Sub

    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return 999999
        End Get
    End Property

    Private TxnResp99999 As New CTxnResp99999
    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return TxnResp99999
        End Get
    End Property

    Public Class CTxnResp99999
        Inherits CStandardResponseBase

        Property MenuContent As String
        Public Overrides Function Encode() As ServerPlatform.Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.Keys.Add("MenuContent")
            Message.Values.Add(MenuContent)

            Return Message
        End Function
    End Class

    <XmlRoot("ul")>
    Public Class CMenuRoot
        <XmlElement("li")>
        Property MenuItems As New List(Of CMenuItem)

        'Class CMenuInfo
        '    <XmlText>
        '    Property TxnDescription As String = ""
        '    <XmlIgnore>
        '    Property FlowId As String = ""
        '    <XmlAttribute("href")>
        '    Property HyperLink As String
        '    Public Sub New(TxnCode As String, Title As String, FlowId As String)
        '        If Not String.IsNullOrEmpty(TxnCode) Then
        '            ' Transaction
        '            Me.TxnDescription = TxnCode + " : " + Title
        '            HyperLink = "javascript::alert('" + TxnCode + "')"
        '        Else
        '            ' Folder
        '            Me.TxnDescription = Title
        '        End If
        '    End Sub
        '    Public Sub New()

        '    End Sub
        'End Class

        Class CMenuItem

            <XmlAttribute("data-txncode")>
            Property TxnCode As String
            <XmlAttribute("data-pageid")>
            Property PageId As String
            <XmlAttribute("data-flowid")>
            Property FlowId As String

            <XmlAttribute("data-nodeid")>
            Property NodeId As String

            <XmlIgnore>
            Property Title As String = ""
            <XmlIgnore>
            Property Id As Integer = 0
            <XmlIgnore>
            Property Parent As Integer = 0

            <XmlElement("a")>
            Property MenuTitle As String
                Get
                    If String.IsNullOrEmpty(TxnCode) Then
                        Return Title
                    Else
                        Return TxnCode + " : " + Title
                    End If

                End Get
                Set(value As String)
                    ' TODO
                End Set
            End Property


            <XmlArray("ul")>
            <XmlArrayItem("li")>
            Property MenuItems As List(Of CMenuItem)

            Public Sub New(ByVal TxnCode As String, ByVal PageId As String, ByVal Title As String, ByVal FlowId As String, ByVal NodeId As String)
                Me.PageId = PageId + ".htm"
                Me.TxnCode = TxnCode
                Me.Title = Title
                Me.FlowId = FlowId
                Me.NodeId = NodeId
            End Sub

            Sub New()

            End Sub

            Public Sub New(ByVal FolderTitle As String, ByVal Id As String, ByVal Parent As String)
                MenuItems = New List(Of CMenuItem)
                Title = FolderTitle
                Me.Id = Id
                Me.Parent = Parent
                ClassName = "folder"
            End Sub
            <XmlAttribute("class")>
            Property ClassName As String

        End Class
    End Class

End Class
