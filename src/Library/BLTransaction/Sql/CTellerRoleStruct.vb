''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTellerRoleStruct.vb
' Class         : CTellerRoleStruct
' Description   : CTellerRole -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTellerRoleStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTellerRoleStruct
    Public Property MaxRecord As Integer
    Public Property TellerId As String
    Public Property TellerRoleId As Integer
    Public Property TellerId_Array As New List(Of String)
    Public Property TellerRoleId_Array As New List(Of Integer)
End Class
