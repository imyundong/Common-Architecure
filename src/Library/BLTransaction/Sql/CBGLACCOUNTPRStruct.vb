''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBGLACCOUNTPRStruct.vb
' Class         : CBGLACCOUNTPRStruct
' Description   : CBGLACCOUNTPR -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBGLACCOUNTPRStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBGLACCOUNTPRStruct
    Public Property MaxRecord As Integer
    Public Property BRANCHCD As String
    Public Property ACCOUNT As String
    Public Property CANELMARK As Char
    Public Property ZEROMARK As Char
    Public Property BRANCHCD_Array As New List(Of String)
    Public Property ACCOUNT_Array As New List(Of String)
    Public Property CANELMARK_Array As New List(Of Char)
    Public Property ZEROMARK_Array As New List(Of Char)
End Class
