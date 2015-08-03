''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTerminalStruct.vb
' Class         : CTerminalStruct
' Description   : CTerminal -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTerminalStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTerminalStruct
    Public Property MaxRecord As Integer
    Public Property TerminalId As Integer
    Public Property BranchId As Integer
    Public Property TerminalId_Array As New List(Of Integer)
    Public Property BranchId_Array As New List(Of Integer)
End Class
