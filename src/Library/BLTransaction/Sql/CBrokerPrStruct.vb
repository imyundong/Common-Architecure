''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBrokerPrStruct.vb
' Class         : CBrokerPrStruct
' Description   : CBrokerPr -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBrokerPrStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBrokerPrStruct
    Public Property MaxRecord As Integer
    Public Property branchcd As String
    Public Property brokercd As String
    Public Property brokername As String
    Public Property branchcd_Array As New List(Of String)
    Public Property brokercd_Array As New List(Of String)
    Public Property brokername_Array As New List(Of String)
End Class
