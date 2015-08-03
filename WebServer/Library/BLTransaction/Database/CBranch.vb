''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBranch.vb
' Class         : CBranch
' Description   : Table : Branch, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBranch.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CBranch
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property BranchId As Integer
    Public Property BranchName As String
    Public Property Parent As Long
    Public Property BranchCategoryId As Integer

    Public Sub New()
        BranchId = 0
        BranchName = ""
        Parent = 0
        BranchCategoryId = 0
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CBranchStruct)
        DBStruct.BranchId = BranchId
    End Sub

    Private Sub ToSql(ByRef DBStruct As CBranchStruct)
        ToSqlPK(DBStruct)
        DBStruct.BranchName = BranchName
        DBStruct.Parent = Parent
        DBStruct.BranchCategoryId = BranchCategoryId
    End Sub

    Private Sub FromSql(ByVal DBStruct As CBranchStruct)
        BranchId = DBStruct.BranchId
        BranchName = DBStruct.BranchName
        Parent = DBStruct.Parent
        BranchCategoryId = DBStruct.BranchCategoryId
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CBranchStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CBranchStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CBranchStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal BranchId As Integer, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CBranchStruct
        Me.BranchId = BranchId
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CBranch))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CBranchStruct In StructObjList
            Dim Obj As New CBranch
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchByParent(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Parent As Long, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CBranchStruct
        StructObj.Parent = Parent
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByParent(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchByBrahchIdGetName(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal BranchId As Integer, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CBranchStruct
        StructObj.BranchId = BranchId
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByBrahchIdGetName(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchByAll(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CBranchStruct
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByAll(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchByBranchCategoryId(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal BranchId As Integer, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CBranchStruct
        StructObj.BranchId = BranchId
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByBranchCategoryId(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CBranchStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "Branch"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim Branch As New CBranch
        Branch.BranchId = BranchId
        Branch.BranchName = BranchName
        Branch.Parent = Parent
        Branch.BranchCategoryId = BranchCategoryId
        Return Branch
    End Function



End Class
