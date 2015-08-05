''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CCARLOAN.vb
' Class         : CCARLOAN
' Description   : Table : CARLOAN, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CCARLOAN.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CCARLOAN
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property ACCTNO As String
    Public Property OLDACCTNO As String

    Public Sub New()
        ACCTNO = ""
        OLDACCTNO = ""
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CCARLOANStruct)
    End Sub

    Private Sub ToSql(ByRef DBStruct As CCARLOANStruct)
        ToSqlPK(DBStruct)
        DBStruct.ACCTNO = ACCTNO
        DBStruct.OLDACCTNO = OLDACCTNO
    End Sub

    Private Sub FromSql(ByVal DBStruct As CCARLOANStruct)
        ACCTNO = DBStruct.ACCTNO
        OLDACCTNO = DBStruct.OLDACCTNO
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CCARLOANStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CCARLOANStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CCARLOANStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CCARLOANStruct
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CCARLOAN))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CCARLOANStruct In StructObjList
            Dim Obj As New CCARLOAN
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchByOLDACCTNO(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal OLDACCTNO As String, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CCARLOANStruct
        StructObj.OLDACCTNO = OLDACCTNO
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByOLDACCTNO(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchByACCTNO(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal ACCTNO As String, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CCARLOANStruct
        StructObj.ACCTNO = ACCTNO
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByACCTNO(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CCARLOANStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "CARLOAN"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim CARLOAN As New CCARLOAN
        CARLOAN.ACCTNO = ACCTNO
        CARLOAN.OLDACCTNO = OLDACCTNO
        Return CARLOAN
    End Function



End Class
