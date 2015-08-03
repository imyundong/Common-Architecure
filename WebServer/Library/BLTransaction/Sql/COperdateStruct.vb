''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COperdateStruct.vb
' Class         : COperdateStruct
' Description   : COperdate -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COperdateStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class COperdateStruct
    Public Property MaxRecord As Integer
    Public Property OP_DATE As String
    Public Property DATE_FLAG As String
    Public Property IS_OP_DATE As String
    Public Property OP_DATE_Array As New List(Of String)
    Public Property DATE_FLAG_Array As New List(Of String)
    Public Property IS_OP_DATE_Array As New List(Of String)
End Class
