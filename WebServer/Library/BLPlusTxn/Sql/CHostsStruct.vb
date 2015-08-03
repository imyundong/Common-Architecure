''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CHostsStruct.vb
' Class         : CHostsStruct
' Description   : CHosts -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CHostsStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CHostsStruct
    Public Property MaxRecord As Integer
    Public Property HostId As Integer
    Public Property HostName As String
    Public Property HostId_Array As New List(Of Integer)
    Public Property HostName_Array As New List(Of String)
End Class
