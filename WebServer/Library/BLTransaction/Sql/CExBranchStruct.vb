''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CExBranchStruct.vb
' Class         : CExBranchStruct
' Description   : CExBranch -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CExBranchStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CExBranchStruct
    Public Property MaxRecord As Integer
    Public Property ExBranchNo As Integer
    Public Property ExBranchName As String
    Public Property ExBranchNo_Array As New List(Of Integer)
    Public Property ExBranchName_Array As New List(Of String)
End Class
