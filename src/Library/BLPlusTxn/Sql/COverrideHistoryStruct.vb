''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : COverrideHistoryStruct.vb
' Class         : COverrideHistoryStruct
' Description   : COverrideHistory -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: COverrideHistoryStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class COverrideHistoryStruct
    Public Property MaxRecord As Integer
    Public Property SequenceNo As Integer
    Public Property OverrideId As String
    Public Property OverrideCode As String
    Public Property UserId As Integer
    Public Property SupervisorId As Integer
    Public Property Status As Integer
    Public Property RequestDate As DateTime
    Public Property UpdateDate As DateTime
    Public Property SequenceNo_Array As New List(Of Integer)
    Public Property OverrideId_Array As New List(Of String)
    Public Property OverrideCode_Array As New List(Of String)
    Public Property UserId_Array As New List(Of Integer)
    Public Property SupervisorId_Array As New List(Of Integer)
    Public Property Status_Array As New List(Of Integer)
    Public Property RequestDate_Array As New List(Of DateTime)
    Public Property UpdateDate_Array As New List(Of DateTime)
End Class
