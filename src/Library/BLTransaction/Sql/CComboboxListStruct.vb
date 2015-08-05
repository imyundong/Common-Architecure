''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CComboboxListStruct.vb
' Class         : CComboboxListStruct
' Description   : CComboboxList -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CComboboxListStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CComboboxListStruct
    Public Property MaxRecord As Integer
    Public Property ComboId As String
    Public Property ComboValue As String
    Public Property ComboDescription As String
    Public Property Priority As Integer
    Public Property ComboId_Array As New List(Of String)
    Public Property ComboValue_Array As New List(Of String)
    Public Property ComboDescription_Array As New List(Of String)
    Public Property Priority_Array As New List(Of Integer)
End Class
