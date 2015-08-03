''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBranchSTStruct.vb
' Class         : CBranchSTStruct
' Description   : CBranchST -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBranchSTStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBranchSTStruct
    Public Property MaxRecord As Integer
    Public Property BANKCD As String
    Public Property BRANCHCD As String
    Public Property UNIONBALANCINGSTATUS As String
    Public Property BALANCINGSTATUS As String
    Public Property BUSINESSDATE As String
    Public Property REMITTANCESTATUS As String
    Public Property FRXBALANCINGSTATUS As String
    Public Property BANKCD_Array As New List(Of String)
    Public Property BRANCHCD_Array As New List(Of String)
    Public Property UNIONBALANCINGSTATUS_Array As New List(Of String)
    Public Property BALANCINGSTATUS_Array As New List(Of String)
    Public Property BUSINESSDATE_Array As New List(Of String)
    Public Property REMITTANCESTATUS_Array As New List(Of String)
    Public Property FRXBALANCINGSTATUS_Array As New List(Of String)
End Class
