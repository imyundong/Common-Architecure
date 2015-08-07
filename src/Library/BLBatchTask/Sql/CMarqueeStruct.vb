''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CMarqueeStruct.vb
' Class         : CMarqueeStruct
' Description   : CMarquee -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CMarqueeStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CMarqueeStruct
    Public Property MaxRecord As Integer
    Public Property SystemId As Integer
    Public Property Content As String
    Public Property SystemId_Array As New List(Of Integer)
    Public Property Content_Array As New List(Of String)
End Class
