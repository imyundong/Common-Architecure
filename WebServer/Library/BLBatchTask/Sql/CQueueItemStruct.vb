''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CQueueItemStruct.vb
' Class         : CQueueItemStruct
' Description   : CQueueItem -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CQueueItemStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CQueueItemStruct
    Public Property MaxRecord As Integer
    Public Property QueueID As Integer
    Public Property Description As String
    Public Property XMLDocument As String
    Public Property Priority As Integer
    Public Property DateAdded As Integer
    Public Property DateProcessed As Integer
    Public Property Status As Integer
    Public Property SenderTellerID As Integer
    Public Property ProcessorTellerID As Integer
    Public Property Capability As Integer
    Public Property TimeAdded As Integer
    Public Property DateExpired As Integer
    Public Property OriginalItemID As Integer
    Public Property QueueID_Array As New List(Of Integer)
    Public Property Description_Array As New List(Of String)
    Public Property XMLDocument_Array As New List(Of String)
    Public Property Priority_Array As New List(Of Integer)
    Public Property DateAdded_Array As New List(Of Integer)
    Public Property DateProcessed_Array As New List(Of Integer)
    Public Property Status_Array As New List(Of Integer)
    Public Property SenderTellerID_Array As New List(Of Integer)
    Public Property ProcessorTellerID_Array As New List(Of Integer)
    Public Property Capability_Array As New List(Of Integer)
    Public Property TimeAdded_Array As New List(Of Integer)
    Public Property DateExpired_Array As New List(Of Integer)
    Public Property OriginalItemID_Array As New List(Of Integer)
End Class
