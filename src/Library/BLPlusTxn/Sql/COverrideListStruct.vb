''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COverrideListStruct.vb
' Class         : COverrideListStruct
' Description   : COverrideList -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COverrideListStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class COverrideListStruct
    Public Property MaxRecord As Integer
    Public Property TxnCode As String
    Public Property BranchCategory As Integer
    Public Property TellerRole As Integer
    Public Property OverrideCode As String
    Public Property TxnCode_Array As New List(Of String)
    Public Property BranchCategory_Array As New List(Of Integer)
    Public Property TellerRole_Array As New List(Of Integer)
    Public Property OverrideCode_Array As New List(Of String)
End Class
