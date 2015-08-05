''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CSystemInfoStruct.vb
' Class         : CSystemInfoStruct
' Description   : CSystemInfo -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CSystemInfoStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CSystemInfoStruct
    Public Property MaxRecord As Integer
    Public Property SystemId As Integer
    Public Property SystemName As String
    Public Property Version As String
    Public Property SystemId_Array As New List(Of Integer)
    Public Property SystemName_Array As New List(Of String)
    Public Property Version_Array As New List(Of String)
End Class
