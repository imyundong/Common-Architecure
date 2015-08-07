''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CCMSKEYStruct.vb
' Class         : CCMSKEYStruct
' Description   : CCMSKEY -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CCMSKEYStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CCMSKEYStruct
    Public Property MaxRecord As Integer
    Public Property KEYID As String
    Public Property KEYVALUE As String
    Public Property KEYID_Array As New List(Of String)
    Public Property KEYVALUE_Array As New List(Of String)
End Class
