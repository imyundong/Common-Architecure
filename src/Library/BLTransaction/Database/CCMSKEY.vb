''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CCMSKEY.vb
' Class         : CCMSKEY
' Description   : Table : CMSKEY, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CCMSKEY.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CCMSKEY
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property KEYID As String
    Public Property KEYVALUE As String

    Public Sub New()
        KEYID = "0"
        KEYVALUE = ""
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CCMSKEYStruct)
        DBStruct.KEYID = KEYID
    End Sub

    Private Sub ToSql(ByRef DBStruct As CCMSKEYStruct)
        ToSqlPK(DBStruct)
        DBStruct.KEYVALUE = KEYVALUE
    End Sub

    Private Sub FromSql(ByVal DBStruct As CCMSKEYStruct)
        KEYID = DBStruct.KEYID
        KEYVALUE = DBStruct.KEYVALUE
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CCMSKEYStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CCMSKEYStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CCMSKEYStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal KEYID As String, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CCMSKEYStruct
        Me.KEYID = KEYID
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CCMSKEY))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CCMSKEYStruct In StructObjList
            Dim Obj As New CCMSKEY
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CCMSKEYStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "CMSKEY"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim CMSKEY As New CCMSKEY
        CMSKEY.KEYID = KEYID
        CMSKEY.KEYVALUE = KEYVALUE
        Return CMSKEY
    End Function



End Class

