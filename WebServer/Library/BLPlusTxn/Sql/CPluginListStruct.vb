''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CPluginListStruct.vb
' Class         : CPluginListStruct
' Description   : CPluginList -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CPluginListStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CPluginListStruct
    Public Property MaxRecord As Integer
    Public Property PluginId As Integer
    Public Property PluginName As String
    Public Property PluginFriendlyName As String
    Public Property PluginIcon As Integer
    Public Property PluginId_Array As New List(Of Integer)
    Public Property PluginName_Array As New List(Of String)
    Public Property PluginFriendlyName_Array As New List(Of String)
    Public Property PluginIcon_Array As New List(Of Integer)
End Class
