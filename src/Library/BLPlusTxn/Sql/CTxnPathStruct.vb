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
    Public Property TxnPathId As Integer
    Public Property TxnPathDescription As String
    Public Property Parent As Integer
    Public Property IconIndex As Integer
    Public Property Priority As Integer
    Public Property TxnPathId_Array As New List(Of Integer)
    Public Property TxnPathDescription_Array As New List(Of String)
    Public Property Parent_Array As New List(Of Integer)
    Public Property IconIndex_Array As New List(Of Integer)
    Public Property Priority_Array As New List(Of Integer)
End Class
