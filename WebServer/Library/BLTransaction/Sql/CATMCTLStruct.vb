''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CATMCTLStruct.vb
' Class         : CATMCTLStruct
' Description   : CATMCTL -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CATMCTLStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CATMCTLStruct
    Public Property MaxRecord As Integer
    Public Property TermId As String
    Public Property BranchId As Integer
    Public Property TelrNo As String
    Public Property ShopLocation As String
    Public Property GroupId As String
    Public Property TermId_Array As New List(Of String)
    Public Property BranchId_Array As New List(Of Integer)
    Public Property TelrNo_Array As New List(Of String)
    Public Property ShopLocation_Array As New List(Of String)
    Public Property GroupId_Array As New List(Of String)
End Class
