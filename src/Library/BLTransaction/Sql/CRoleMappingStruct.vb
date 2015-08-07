''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CRoleMappingStruct.vb
' Class         : CRoleMappingStruct
' Description   : CRoleMapping -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CRoleMappingStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CRoleMappingStruct
    Public Property MaxRecord As Integer
    Public Property ETabRole As String
    Public Property TellerRoleId As Integer
    Public Property ETabRole_Array As New List(Of String)
    Public Property TellerRoleId_Array As New List(Of Integer)
End Class
