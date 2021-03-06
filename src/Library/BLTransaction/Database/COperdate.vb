''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COperdate.vb
' Class         : COperdate
' Description   : Table : Operdate, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COperdate.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class COperdate
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property OPDATE As String
    Public Property DATEFLAG As String
    Public Property ISOPDATE As String

    Public Sub New()
        OPDATE = "0"
        DATEFLAG = ""
        ISOPDATE = ""
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As COperdateStruct)
    End Sub

    Private Sub ToSql(ByRef DBStruct As COperdateStruct)
        ToSqlPK(DBStruct)
        DBStruct.OP_DATE = OPDATE
        DBStruct.DATE_FLAG = DATEFLAG
        DBStruct.IS_OP_DATE = ISOPDATE
    End Sub

    Private Sub FromSql(ByVal DBStruct As COperdateStruct)
        OPDATE = DBStruct.OP_DATE
        DATEFLAG = DBStruct.DATE_FLAG
        ISOPDATE = DBStruct.IS_OP_DATE
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New COperdateStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New COperdateStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New COperdateStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New COperdateStruct
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of COperdate))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As COperdateStruct In StructObjList
            Dim Obj As New COperdate
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchByOperDate(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal OP_DATE As String, _
                    ByVal IS_OP_DATE As String, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New COperdateStruct
        StructObj.OP_DATE = OP_DATE
        StructObj.IS_OP_DATE = IS_OP_DATE
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByOperDate(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchNextDateByOperDate(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal OP_DATE As String, _
                    ByVal IS_OP_DATE As String, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New COperdateStruct
        StructObj.OP_DATE = OP_DATE
        StructObj.IS_OP_DATE = IS_OP_DATE
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchNextDateByOperDate(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchPreDateByOperDate(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal OP_DATE As String, _
                    ByVal IS_OP_DATE As String, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New COperdateStruct
        StructObj.OP_DATE = OP_DATE
        StructObj.IS_OP_DATE = IS_OP_DATE
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchPreDateByOperDate(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New COperdateStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "Operdate"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim Operdate As New COperdate
        Operdate.OPDATE = OPDATE
        Operdate.DATEFLAG = DATEFLAG
        Operdate.ISOPDATE = ISOPDATE
        Return Operdate
    End Function



End Class

