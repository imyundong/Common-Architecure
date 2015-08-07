''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CCOMPANYNOStruct.vb
' Class         : CCOMPANYNOStruct
' Description   : CCOMPANYNO -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CCOMPANYNOStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CCOMPANYNOStruct
    Public Property MaxRecord As Integer
    Public Property PAYROLLCODE As String
    Public Property COMPANYID As String
    Public Property COMPANYNAME As String
    Public Property PAYROLLCODE_Array As New List(Of String)
    Public Property COMPANYID_Array As New List(Of String)
    Public Property COMPANYNAME_Array As New List(Of String)
End Class
