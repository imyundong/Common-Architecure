''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBroadcastNotificationStruct.vb
' Class         : CBroadcastNotificationStruct
' Description   : CBroadcastNotification -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBroadcastNotificationStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBroadcastNotificationStruct
    Public Property MaxRecord As Integer
    Public Property TellerID As String
    Public Property Sender As Integer
    Public Property SystemDate As DateTime
    Public Property TellerID_Array As New List(Of String)
    Public Property Sender_Array As New List(Of Integer)
    Public Property SystemDate_Array As New List(Of DateTime)
End Class
