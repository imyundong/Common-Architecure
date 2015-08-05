''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CJournalStruct.vb
' Class         : CJournalStruct
' Description   : CJournal -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CJournalStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CJournalStruct
    Public Property MaxRecord As Integer
    Public Property JournalId As Integer
    Public Property GroupId As String
    Public Property TxnCode As String
    Public Property Status As Integer
    Public Property ErrCode As String
    Public Property ErrDescription As String
    Public Property HostId As String
    Public Property TraceNo As String
    Public Property SystemDate As DateTime
    Public Property BusinessDate As DateTime
    Public Property UserId As Integer
    Public Property BranchId As Integer
    Public Property Supervisor As Integer
    Public Property Account As String
    Public Property Amount As Double
    Public Property Currency As String
    Public Property Request As String
    Public Property Response As String
    Public Property Terminal As Integer
    Public Property ProcTime As Long
    Public Property PageData As String
    Public Property OverrideId As String
    Public Property JournalId_Array As New List(Of Integer)
    Public Property GroupId_Array As New List(Of String)
    Public Property TxnCode_Array As New List(Of String)
    Public Property Status_Array As New List(Of Integer)
    Public Property ErrCode_Array As New List(Of String)
    Public Property ErrDescription_Array As New List(Of String)
    Public Property HostId_Array As New List(Of String)
    Public Property TraceNo_Array As New List(Of String)
    Public Property SystemDate_Array As New List(Of DateTime)
    Public Property BusinessDate_Array As New List(Of DateTime)
    Public Property UserId_Array As New List(Of Integer)
    Public Property BranchId_Array As New List(Of Integer)
    Public Property Supervisor_Array As New List(Of Integer)
    Public Property Account_Array As New List(Of String)
    Public Property Amount_Array As New List(Of Double)
    Public Property Currency_Array As New List(Of String)
    Public Property Request_Array As New List(Of String)
    Public Property Response_Array As New List(Of String)
    Public Property Terminal_Array As New List(Of Integer)
    Public Property ProcTime_Array As New List(Of Long)
    Public Property PageData_Array As New List(Of String)
    Public Property OverrideId_Array As New List(Of String)
End Class
