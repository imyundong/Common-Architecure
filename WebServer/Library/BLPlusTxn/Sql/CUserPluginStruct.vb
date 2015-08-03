''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CUserPluginStruct.vb
' Class         : CUserPluginStruct
' Description   : CUserPlugin -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CUserPluginStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CUserPluginStruct
    Public Property MaxRecord As Integer
    Public Property UserId As Integer
    Public Property PluginId As Integer
    Public Property UserId_Array As New List(Of Integer)
    Public Property PluginId_Array As New List(Of Integer)
End Class
