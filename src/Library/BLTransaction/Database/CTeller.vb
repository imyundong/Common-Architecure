''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTeller.vb
' Class         : CTeller
' Description   : Table : Teller, This Class is Generated By DBCG Tools Automatically
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTeller.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Library.Workflow

Public Class CTeller
    Implements ICloneable
    Private Curser As New List(Of Object)

    Public Property TellerId As String
    Public Property BranchId As Integer
    Public Property Enabled As Integer
    Public Property TellerName As String
    Public Property TerminalId As Integer
    Public Property Capability As Integer
    Public Property HostTellerId As Integer
    Public Property LeaveDate As String

    Public Sub New()
        TellerId = "0"
        BranchId = 0
        Enabled = 0
        TellerName = "''"
        TerminalId = 0
        Capability = 0
        HostTellerId = 0
        LeaveDate = "0"
    End Sub

    Private Sub ToSqlPK(ByRef DBStruct As CTellerStruct)
        DBStruct.TellerId = TellerId
    End Sub

    Private Sub ToSql(ByRef DBStruct As CTellerStruct)
        ToSqlPK(DBStruct)
        DBStruct.BranchId = BranchId
        DBStruct.Enabled = Enabled
        DBStruct.TellerName = TellerName
        DBStruct.TerminalId = TerminalId
        DBStruct.Capability = Capability
        DBStruct.HostTellerId = HostTellerId
        DBStruct.LeaveDate = LeaveDate
    End Sub

    Private Sub FromSql(ByVal DBStruct As CTellerStruct)
        TellerId = DBStruct.TellerId
        BranchId = DBStruct.BranchId
        Enabled = DBStruct.Enabled
        TellerName = DBStruct.TellerName
        TerminalId = DBStruct.TerminalId
        Capability = DBStruct.Capability
        HostTellerId = DBStruct.HostTellerId
        LeaveDate = DBStruct.LeaveDate
    End Sub

    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTellerStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTellerStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)
        Dim StructObj As New CTellerStruct
        ToSql(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)
    End Sub

    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal TellerId As String, _
                    ByVal Lock As Boolean?)

        Dim StructObj As New CTellerStruct
        Me.TellerId = TellerId
        ToSqlPK(StructObj)

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)
        FromSql(StructObj)
    End Sub

    Public Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of CTeller))

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)

        Dim StructObjList As New List(Of Object)
        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)

        For Each StructObj As CTellerStruct In StructObjList
            Dim Obj As New CTeller
            Obj.FromSql(StructObj)
            ObjList.Add(Obj)
        Next
    End Sub


    Public Sub SearchByEnabled(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Enabled As Integer, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CTellerStruct
        StructObj.Enabled = Enabled
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByEnabled(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Sub SearchBySameBranchEnabledCpaOver5(ByRef DatabaseFactory As CDatabaseFactory, _
                    ByVal Enabled As Integer, _
                    ByVal BranchId As Integer, _
                    ByVal Capability As Integer, _
                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)
        If Curser.Count > 0 Then
            If Index <= Curser.Count - 1 Then
                FromSql(Curser.Item(Index))
                Exit Sub
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, "Index Out Of Range")
            End If
        End If

        Dim StructObj As New CTellerStruct
        StructObj.Enabled = Enabled
        StructObj.BranchId = BranchId
        StructObj.Capability = Capability
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchBySameBranchEnabledCpaOver5(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

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

        Dim StructObj As New CTellerStruct
        StructObj.BranchId = BranchId
        StructObj.MaxRecord = MaxRecord

        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        DBAccessClass.SearchByBranch(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)

        FromSql(Curser.Item(Index))

    End Sub

    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer

        Dim StructObj As New CTellerStruct

        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)
        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)
    End Function

    Public ReadOnly Property ClassName As String
        Get
            Return "Teller"
        End Get
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim Teller As New CTeller
        Teller.TellerId = TellerId
        Teller.BranchId = BranchId
        Teller.Enabled = Enabled
        Teller.TellerName = TellerName
        Teller.TerminalId = TerminalId
        Teller.Capability = Capability
        Teller.HostTellerId = HostTellerId
        Teller.LeaveDate = LeaveDate
        Return Teller
    End Function



End Class

