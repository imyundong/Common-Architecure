''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBusinessGroupStruct.vb
' Class         : CBusinessGroupStruct
' Description   : CBusinessGroup -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBusinessGroupStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBusinessGroupStruct
    Public Property MaxRecord As Integer
    Public Property BusinessGroupId As Integer
    Public Property TxnCode As String
    Public Property BusinessGroupId_Array As New List(Of Integer)
    Public Property TxnCode_Array As New List(Of String)
End Class
