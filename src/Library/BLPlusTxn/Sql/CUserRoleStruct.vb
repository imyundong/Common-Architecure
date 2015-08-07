''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CUserRoleStruct.vb
' Class         : CUserRoleStruct
' Description   : CUserRole -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CUserRoleStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CUserRoleStruct
    Public Property MaxRecord As Integer
    Public Property UserId As Integer
    Public Property RoleId As Integer
    Public Property UserId_Array As New List(Of Integer)
    Public Property RoleId_Array As New List(Of Integer)
End Class
