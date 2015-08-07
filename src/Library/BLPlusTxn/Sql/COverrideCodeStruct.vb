''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COverrideCodeStruct.vb
' Class         : COverrideCodeStruct
' Description   : COverrideCode -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COverrideCodeStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class COverrideCodeStruct
    Public Property MaxRecord As Integer
    Public Property Code As String
    Public Property OverrideDescription As String
    Public Property Capability As Integer
    Public Property Condition As String
    Public Property Code_Array As New List(Of String)
    Public Property OverrideDescription_Array As New List(Of String)
    Public Property Capability_Array As New List(Of Integer)
    Public Property Condition_Array As New List(Of String)
End Class
