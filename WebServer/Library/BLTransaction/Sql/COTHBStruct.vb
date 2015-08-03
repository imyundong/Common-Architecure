''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COTHBStruct.vb
' Class         : COTHBStruct
' Description   : COTHB -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COTHBStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class COTHBStruct
    Public Property MaxRecord As Integer
    Public Property BankCode As String
    Public Property BranchCode As String
    Public Property BranchFullName As String
    Public Property BranchShortName As String
    Public Property BankCode_Array As New List(Of String)
    Public Property BranchCode_Array As New List(Of String)
    Public Property BranchFullName_Array As New List(Of String)
    Public Property BranchShortName_Array As New List(Of String)
End Class
