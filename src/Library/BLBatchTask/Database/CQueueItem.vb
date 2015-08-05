''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CQueueItem.vb
' Class         : CQueueItem
' Description   : Table : QueueItem, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CQueueItem.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CQueueItem
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property QueueID As Integer
    Public Property Description As String
    Public Property XMLDocument As String
    Public Property Priority As Integer
    Public Property DateAdded As Integer
    Public Property DateProcessed As Integer
    Public Property Status As Integer
    Public Property SenderTellerID As Integer
    Public Property ProcessorTellerID As Integer
    Public Property Capability As Integer
    Public Property TimeAdded As Integer
    Public Property DateExpired As Integer
    Public Property OriginalItemID As Integer

    Public Sub New()
        QueueID = 0
        Description = ""
        XMLDocument = ""
        Priority = 0
        DateAdded = 0
        DateProcessed = 0
        Status = 0
        SenderTellerID = 0
        ProcessorTellerID = 0
        Capability = 0
        TimeAdded = 0
        DateExpired = 0
        OriginalItemID = 0
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CQueueItemStruct)
    End Sub

    Private Sub ToSql(ByRef DBStruct As CQueueItemStruct)
        ToSqlPK(DBStruct)
        DBStruct.QueueID = QueueID
        DBStruct.Description = Description
        DBStruct.XMLDocument = XMLDocument
        DBStruct.Priority = Priority
        DBStruct.DateAdded = DateAdded
        DBStruct.DateProcessed = DateProcessed
        DBStruct.Status = Status
        DBStruct.SenderTellerID = SenderTellerID
        DBStruct.ProcessorTellerID = ProcessorTellerID
        DBStruct.Capability = Capability
        DBStruct.TimeAdded = TimeAdded
        DBStruct.DateExpired = DateExpired
        DBStruct.OriginalItemID = OriginalItemID
    End Sub

    Private Sub FromSql(ByVal DBStruct As CQueueItemStruct)
        QueueID = DBStruct.QueueID
        Description = DBStruct.Description
        XMLDocument = DBStruct.XMLDocument
        Priority = DBStruct.Priority
        DateAdded = DBStruct.DateAdded
        DateProcessed = DBStruct.DateProcessed
        Status = DBStruct.Status
        SenderTellerID = DBStruct.SenderTellerID
        ProcessorTellerID = DBStruct.ProcessorTellerID
        Capability = DBStruct.Capability
        TimeAdded = DBStruct.TimeAdded
        DateExpired = DBStruct.DateExpired
        OriginalItemID = DBStruct.OriginalItemID
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CQueueItemStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CQueueItemStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CQueueItemStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CQueueItemStruct
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CQueueItem))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CQueueItemStruct In StructObjList
            Dim Obj As New CQueueItem
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CQueueItemStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "QueueItem"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim QueueItem As New CQueueItem
        QueueItem.QueueID = QueueID
        QueueItem.Description = Description
        QueueItem.XMLDocument = XMLDocument
        QueueItem.Priority = Priority
        QueueItem.DateAdded = DateAdded
        QueueItem.DateProcessed = DateProcessed
        QueueItem.Status = Status
        QueueItem.SenderTellerID = SenderTellerID
        QueueItem.ProcessorTellerID = ProcessorTellerID
        QueueItem.Capability = Capability
        QueueItem.TimeAdded = TimeAdded
        QueueItem.DateExpired = DateExpired
        QueueItem.OriginalItemID = OriginalItemID
        Return QueueItem
    End Function



End Class
