''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CSignOnSupervisorStruct.vb
' Class         : CSignOnSupervisorStruct
' Description   : CSignOnSupervisor -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CSignOnSupervisorStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CSignOnSupervisorStruct
    Public Property MaxRecord As Integer
    Public Property TellerId As String
    Public Property BranchId As Integer
    Public Property CurrentBranch As Integer
    Public Property TellerRoleId As Integer
    Public Property Enabled As Integer
    Public Property TellerName As String
    Public Property TellerId_Array As New List(Of String)
    Public Property BranchId_Array As New List(Of Integer)
    Public Property CurrentBranch_Array As New List(Of Integer)
    Public Property TellerRoleId_Array As New List(Of Integer)
    Public Property Enabled_Array As New List(Of Integer)
    Public Property TellerName_Array As New List(Of String)
End Class
