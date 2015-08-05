''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTellerStateStruct.vb
' Class         : CTellerStateStruct
' Description   : CTellerState -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTellerStateStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTellerStateStruct
    Public Property MaxRecord As Integer
    Public Property TellerID As String
    Public Property TellerRoleID As Integer
    Public Property CurrentBranch As Integer
    Public Property ParentBranch As Integer
    Public Property TellerID_Array As New List(Of String)
    Public Property TellerRoleID_Array As New List(Of Integer)
    Public Property CurrentBranch_Array As New List(Of Integer)
    Public Property ParentBranch_Array As New List(Of Integer)
End Class
