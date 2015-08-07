''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CUserRoleCategoryStruct.vb
' Class         : CUserRoleCategoryStruct
' Description   : CUserRoleCategory -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CUserRoleCategoryStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CUserRoleCategoryStruct
    Public Property MaxRecord As Integer
    Public Property RoleId As Integer
    Public Property RoleDescription As String
    Public Property Capability As Integer
    Public Property RoleGroup As Integer
    Public Property RoleAttribuite As String
    Public Property RoleId_Array As New List(Of Integer)
    Public Property RoleDescription_Array As New List(Of String)
    Public Property Capability_Array As New List(Of Integer)
    Public Property RoleGroup_Array As New List(Of Integer)
    Public Property RoleAttribuite_Array As New List(Of String)
End Class
