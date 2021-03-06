''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CATMCTL.vb
' Class         : CATMCTL
' Description   : Table : ATMCTL, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CATMCTL.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CATMCTL
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property TermId As String
    Public Property BranchId As Integer
    Public Property TelrNo As String
    Public Property ShopLocation As String
    Public Property GroupId As String

    Public Sub New()
        TermId = ""
        BranchId = 0
        TelrNo = ""
        ShopLocation = ""
        GroupId = ""
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CATMCTLStruct)
        DBStruct.TermId = TermId
        DBStruct.BranchId = BranchId
        DBStruct.TelrNo = TelrNo
    End Sub

    Private Sub ToSql(ByRef DBStruct As CATMCTLStruct)
        ToSqlPK(DBStruct)
        DBStruct.ShopLocation = ShopLocation
        DBStruct.GroupId = GroupId
    End Sub

    Private Sub FromSql(ByVal DBStruct As CATMCTLStruct)
        TermId = DBStruct.TermId
        BranchId = DBStruct.BranchId
        TelrNo = DBStruct.TelrNo
        ShopLocation = DBStruct.ShopLocation
        GroupId = DBStruct.GroupId
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CATMCTLStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CATMCTLStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CATMCTLStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal TermId As String, _
                    ByVal BranchId As Integer, _
                    ByVal TelrNo As String, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CATMCTLStruct
        Me.TermId = TermId
        Me.BranchId = BranchId
        Me.TelrNo = TelrNo
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CATMCTL))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CATMCTLStruct In StructObjList
            Dim Obj As New CATMCTL
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchByBranch(ByRef DatabaseFactory As CDatabaseFactory, _
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

        Dim StructObj As New CATMCTLStruct
        StructObj.BranchId = BranchId
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByBranch(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CATMCTLStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "ATMCTL"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim ATMCTL As New CATMCTL
        ATMCTL.TermId = TermId
        ATMCTL.BranchId = BranchId
        ATMCTL.TelrNo = TelrNo
        ATMCTL.ShopLocation = ShopLocation
        ATMCTL.GroupId = GroupId
        Return ATMCTL
    End Function



End Class

