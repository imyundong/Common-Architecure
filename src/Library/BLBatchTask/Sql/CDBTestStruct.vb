''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CDBTestStruct.vb
' Class         : CDBTestStruct
' Description   : CDBTest -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CDBTestStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CDBTestStruct
    Public Property MaxRecord As Integer
    Public Property TestId As Integer
    Public Property TestValue As String
    Public Property TestDate As DateTime
    Public Property TestId_Array As New List(Of Integer)
    Public Property TestValue_Array As New List(Of String)
    Public Property TestDate_Array As New List(Of DateTime)
End Class
