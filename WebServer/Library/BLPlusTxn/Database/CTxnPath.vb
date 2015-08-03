''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTxnPath.vb
' Class         : CTxnPath
' Description   : Table : TxnPath, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTxnPath.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CTxnPath
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property TxnPathId As Integer
    Public Property TxnPathDescription As String
    Public Property Parent As Integer
    Public Property IconIndex As Integer
    Public Property Priority As Integer

    Public Sub New()
        TxnPathId = 0
        TxnPathDescription = "0"
        Parent = 0
        IconIndex = 0
        Priority = 0
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CTxnPathStruct)
        DBStruct.TxnPathId = TxnPathId
    End Sub

    Private Sub ToSql(ByRef DBStruct As CTxnPathStruct)
        ToSqlPK(DBStruct)
        DBStruct.TxnPathDescription = TxnPathDescription
        DBStruct.Parent = Parent
        DBStruct.IconIndex = IconIndex
        DBStruct.Priority = Priority
    End Sub

    Private Sub FromSql(ByVal DBStruct As CTxnPathStruct)
        TxnPathId = DBStruct.TxnPathId
        TxnPathDescription = DBStruct.TxnPathDescription
        Parent = DBStruct.Parent
        IconIndex = DBStruct.IconIndex
        Priority = DBStruct.Priority
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTxnPathStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTxnPathStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTxnPathStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Remove(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal TxnPathId As Integer, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CTxnPathStruct
        Me.TxnPathId = TxnPathId
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Shared Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CTxnPath))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary("TxnPath")

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CTxnPathStruct In StructObjList
            Dim Obj As New CTxnPath
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchOrderByPriority(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CTxnPathStruct
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchOrderByPriority(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CTxnPathStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "TxnPath"
        End Get
    End Property

    Public Overridable Function Clone() As Object Implements ICloneable.Clone
        Dim TxnPath As New CTxnPath
        TxnPath.TxnPathId = TxnPathId
        TxnPath.TxnPathDescription = TxnPathDescription
        TxnPath.Parent = Parent
        TxnPath.IconIndex = IconIndex
        TxnPath.Priority = Priority
        Return TxnPath
    End Function



End Class

