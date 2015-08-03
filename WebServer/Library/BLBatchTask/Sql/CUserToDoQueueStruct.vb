''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CUserToDoQueueStruct.vb
' Class         : CUserToDoQueueStruct
' Description   : CUserToDoQueue -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CUserToDoQueueStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CUserToDoQueueStruct
    Public Property MaxRecord As Integer
    Public Property QueueID As Integer
    Public Property DeviceID As Integer
    Public Property GroupID As Integer
    Public Property ID As Integer
    Public Property IDType As String
    Public Property QueueID_Array As New List(Of Integer)
    Public Property DeviceID_Array As New List(Of Integer)
    Public Property GroupID_Array As New List(Of Integer)
    Public Property ID_Array As New List(Of Integer)
    Public Property IDType_Array As New List(Of String)
End Class
