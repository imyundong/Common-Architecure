''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTellerStruct.vb
' Class         : CTellerStruct
' Description   : CTeller -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTellerStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTellerStruct
    Public Property MaxRecord As Integer
    Public Property TellerId As String
    Public Property BranchId As Integer
    Public Property Enabled As Integer
    Public Property TellerName As String
    Public Property TerminalId As Integer
    Public Property Capability As Integer
    Public Property HostTellerId As Integer
    Public Property LeaveDate As String
    Public Property TellerId_Array As New List(Of String)
    Public Property BranchId_Array As New List(Of Integer)
    Public Property Enabled_Array As New List(Of Integer)
    Public Property TellerName_Array As New List(Of String)
    Public Property TerminalId_Array As New List(Of Integer)
    Public Property Capability_Array As New List(Of Integer)
    Public Property HostTellerId_Array As New List(Of Integer)
    Public Property LeaveDate_Array As New List(Of String)
End Class
