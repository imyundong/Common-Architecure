''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CNotificationSummaryStruct.vb
' Class         : CNotificationSummaryStruct
' Description   : CNotificationSummary -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CNotificationSummaryStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CNotificationSummaryStruct
    Public Property MaxRecord As Integer
    Public Property TellerId As String
    Public Property IBDNotificationCount As Integer
    Public Property PassThruNotificationCount As Integer
    Public Property BroadcastNotificationCount As Integer
    Public Property TellerId_Array As New List(Of String)
    Public Property IBDNotificationCount_Array As New List(Of Integer)
    Public Property PassThruNotificationCount_Array As New List(Of Integer)
    Public Property BroadcastNotificationCount_Array As New List(Of Integer)
End Class
