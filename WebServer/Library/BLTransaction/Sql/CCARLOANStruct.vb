''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CCARLOANStruct.vb
' Class         : CCARLOANStruct
' Description   : CCARLOAN -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CCARLOANStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CCARLOANStruct
    Public Property MaxRecord As Integer
    Public Property ACCTNO As String
    Public Property OLDACCTNO As String
    Public Property ACCTNO_Array As New List(Of String)
    Public Property OLDACCTNO_Array As New List(Of String)
End Class
