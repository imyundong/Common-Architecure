''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTransactionPermissionStruct.vb
' Class         : CTransactionPermissionStruct
' Description   : CTransactionPermission -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTransactionPermissionStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTransactionPermissionStruct
    Public Property MaxRecord As Integer
    Public Property BranchCategoryId As Integer
    Public Property TellerRoleId As Integer
    Public Property BusinessGroupId As Integer
    Public Property BranchCategoryId_Array As New List(Of Integer)
    Public Property TellerRoleId_Array As New List(Of Integer)
    Public Property BusinessGroupId_Array As New List(Of Integer)
End Class
