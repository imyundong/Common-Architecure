''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBranchStruct.vb
' Class         : CBranchStruct
' Description   : CBranch -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBranchStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBranchStruct
    Public Property MaxRecord As Integer
    Public Property BranchId As Integer
    Public Property BranchStatus As String
    Public Property BranchId_Array As New List(Of Integer)
    Public Property BranchStatus_Array As New List(Of String)
End Class
