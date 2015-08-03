''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTransactionPermission.vb
' Class         : CTransactionPermission
' Description   : Table : TransactionPermission, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTransactionPermission.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CTransactionPermission
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property BranchCategoryId As Integer
    Public Property TellerRoleId As Integer
    Public Property BusinessGroupId As Integer

    Public Sub New()
        BranchCategoryId = 0
        TellerRoleId = 0
        BusinessGroupId = 0
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CTransactionPermissionStruct)
    End Sub

    Private Sub ToSql(ByRef DBStruct As CTransactionPermissionStruct)
        ToSqlPK(DBStruct)
        DBStruct.BranchCategoryId = BranchCategoryId
        DBStruct.TellerRoleId = TellerRoleId
        DBStruct.BusinessGroupId = BusinessGroupId
    End Sub

    Private Sub FromSql(ByVal DBStruct As CTransactionPermissionStruct)
        BranchCategoryId = DBStruct.BranchCategoryId
        TellerRoleId = DBStruct.TellerRoleId
        BusinessGroupId = DBStruct.BusinessGroupId
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTransactionPermissionStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTransactionPermissionStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTransactionPermissionStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Remove(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CTransactionPermissionStruct
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Shared Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CTransactionPermission))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary("TransactionPermission")

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CTransactionPermissionStruct In StructObjList
            Dim Obj As New CTransactionPermission
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchByBranchAndTellerRole(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal BranchCategoryId As Integer, _
                    ByVal TellerRoleId As List(Of Integer), _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CTransactionPermissionStruct
        StructObj.BranchCategoryId = BranchCategoryId
        StructObj.TellerRoleId_Array = TellerRoleId
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByBranchAndTellerRole(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CTransactionPermissionStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "TransactionPermission"
        End Get
    End Property

    Public Overridable Function Clone() As Object Implements ICloneable.Clone
        Dim TransactionPermission As New CTransactionPermission
        TransactionPermission.BranchCategoryId = BranchCategoryId
        TransactionPermission.TellerRoleId = TellerRoleId
        TransactionPermission.BusinessGroupId = BusinessGroupId
        Return TransactionPermission
    End Function



End Class
