''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CIBDNotifySummaryStruct.vb
' Class         : CIBDNotifySummaryStruct
' Description   : CIBDNotifySummary -> Database Type
' Author        : Su Jia
' Creation Date : 2015/4/13
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CIBDNotifySummaryStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CIBDNotifySummaryStruct
    Public Property MaxRecord As Integer
    Public Property TellerId As String
    Public Property NotificationCount As Integer
    Public Property TellerId_Array As New List(Of String)
    Public Property NotificationCount_Array As New List(Of Integer)
End Class
