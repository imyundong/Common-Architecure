''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTxnPathStruct.vb
' Class         : CTxnPathStruct
' Description   : CTxnPath -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTxnPathStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTxnPathStruct
    Public Property MaxRecord As Integer
    Public Property TxnPathId As Short
    Public Property ParentId As Short
    Public Property PathName As String
    Public Property Priority As Short
    Public Property TxnPathId_Array As New List(Of Short)
    Public Property ParentId_Array As New List(Of Short)
    Public Property PathName_Array As New List(Of String)
    Public Property Priority_Array As New List(Of Short)
End Class
