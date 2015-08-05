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
    Public Property BranchCategory As Integer
    Public Property Parent As Integer
    Public Property ProvincialBranch As Integer

    Public Sub New()
        BranchId = 0
        BranchName = ""
        BranchCategory = 0
        Parent = 0
        ProvincialBranch = 0
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CBranchStruct)
        DBStruct.BranchId = BranchId
    End Sub

    Private Sub ToSql(ByRef DBStruct As CBranchStruct)
        ToSqlPK(DBStruct)
        DBStruct.BranchName = BranchName
        DBStruct.BranchCategory = BranchCategory
        DBStruct.Parent = Parent
        DBStruct.ProvincialBranch = ProvincialBranch
    End Sub

    Private Sub FromSql(ByVal DBStruct As CBranchStruct)
        BranchId = DBStruct.BranchId
        BranchName = DBStruct.BranchName
        BranchCategory = DBStruct.BranchCategory
        Parent = DBStruct.Parent
        ProvincialBranch = DBStruct.ProvincialBranch
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
        DBAccessClass.Remove(DatabaseFactory.CreateInstance, StructObj)
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

    Public Shared Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CBranch))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary("Branch")

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CBranchStruct In StructObjList
            Dim Obj As New CBranch
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
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

    Public Overridable Function Clone() As Object Implements ICloneable.Clone
        Dim Branch As New CBranch
        Branch.BranchId = BranchId
        Branch.BranchName = BranchName
        Branch.BranchCategory = BranchCategory
        Branch.Parent = Parent
        Branch.ProvincialBranch = ProvincialBranch
        Return Branch
    End Function



End Class

