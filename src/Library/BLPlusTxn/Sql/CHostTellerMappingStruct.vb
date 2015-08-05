''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CHostTellerMappingStruct.vb
' Class         : CHostTellerMappingStruct
' Description   : CHostTellerMapping -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CHostTellerMappingStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CHostTellerMappingStruct
    Public Property MaxRecord As Integer
    Public Property UserId As Integer
    Public Property HostId As Integer
    Public Property HostTeller As String
    Public Property UserId_Array As New List(Of Integer)
    Public Property HostId_Array As New List(Of Integer)
    Public Property HostTeller_Array As New List(Of String)
End Class
