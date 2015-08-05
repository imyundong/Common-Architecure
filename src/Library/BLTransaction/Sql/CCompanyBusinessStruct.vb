''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CCompanyBusinessStruct.vb
' Class         : CCompanyBusinessStruct
' Description   : CCompanyBusiness -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CCompanyBusinessStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CCompanyBusinessStruct
    Public Property MaxRecord As Integer
    Public Property PayrollCode As String
    Public Property CompanyId As String
    Public Property CompanyName As String
    Public Property TransactionType As String
    Public Property PayrollCode_Array As New List(Of String)
    Public Property CompanyId_Array As New List(Of String)
    Public Property CompanyName_Array As New List(Of String)
    Public Property TransactionType_Array As New List(Of String)
End Class
