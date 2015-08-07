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
    Public Property BranchName As String
    Public Property BranchCategory As Integer
    Public Property Parent As Integer
    Public Property ProvincialBranch As Integer
    Public Property BranchId_Array As New List(Of Integer)
    Public Property BranchName_Array As New List(Of String)
    Public Property BranchCategory_Array As New List(Of Integer)
    Public Property Parent_Array As New List(Of Integer)
    Public Property ProvincialBranch_Array As New List(Of Integer)
End Class
