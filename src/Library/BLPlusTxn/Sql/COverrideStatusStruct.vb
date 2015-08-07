''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COverrideStatusStruct.vb
' Class         : COverrideStatusStruct
' Description   : COverrideStatus -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COverrideStatusStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class COverrideStatusStruct
    Public Property MaxRecord As Integer
    Public Property OverrideStatusId As Integer
    Public Property Description As String
    Public Property OverrideStatusId_Array As New List(Of Integer)
    Public Property Description_Array As New List(Of String)
End Class
