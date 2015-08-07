''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CJournalViewStruct.vb
' Class         : CJournalViewStruct
' Description   : CJournalView -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CJournalViewStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CJournalViewStruct
    Public Property MaxRecord As Integer
    Public Property JournalId As Integer
    Public Property GroupId As String
    Public Property TxnCode As String
    Public Property Title As String
    Public Property Status As Integer
    Public Property StatusDescription As String
    Public Property ErrCode As String
    Public Property ErrDescription As String
    Public Property ProcTime As String
    Public Property HostId As Integer
    Public Property HostName As String
    Public Property TraceNo As Integer
    Public Property SystemDate As DateTime
    Public Property BusinessDate As DateTime
    Public Property Teller As Integer
    Public Property TellerName As String
    Public Property BranchId As Integer
    Public Property Supervisor As Integer
    Public Property SupervisorName As String
    Public Property Account As String
    Public Property Currency As String
    Public Property TxnAmount As Double
    Public Property Terminal As Integer
    Public Property PageData As String
    Public Property OverrideId As String
    Public Property ReversalTxn As String
    Public Property JournalId_Array As New List(Of Integer)
    Public Property GroupId_Array As New List(Of String)
    Public Property TxnCode_Array As New List(Of String)
    Public Property Title_Array As New List(Of String)
    Public Property Status_Array As New List(Of Integer)
    Public Property StatusDescription_Array As New List(Of String)
    Public Property ErrCode_Array As New List(Of String)
    Public Property ErrDescription_Array As New List(Of String)
    Public Property ProcTime_Array As New List(Of String)
    Public Property HostId_Array As New List(Of Integer)
    Public Property HostName_Array As New List(Of String)
    Public Property TraceNo_Array As New List(Of Integer)
    Public Property SystemDate_Array As New List(Of DateTime)
    Public Property BusinessDate_Array As New List(Of DateTime)
    Public Property Teller_Array As New List(Of Integer)
    Public Property TellerName_Array As New List(Of String)
    Public Property BranchId_Array As New List(Of Integer)
    Public Property Supervisor_Array As New List(Of Integer)
    Public Property SupervisorName_Array As New List(Of String)
    Public Property Account_Array As New List(Of String)
    Public Property Currency_Array As New List(Of String)
    Public Property TxnAmount_Array As New List(Of Double)
    Public Property Terminal_Array As New List(Of Integer)
    Public Property PageData_Array As New List(Of String)
    Public Property OverrideId_Array As New List(Of String)
    Public Property ReversalTxn_Array As New List(Of String)
End Class
