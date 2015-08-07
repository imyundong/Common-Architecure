''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CTxnParameterStruct.vb
' Class         : CTxnParameterStruct
' Description   : CTxnParameter -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CTxnParameterStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CTxnParameterStruct
    Public Property MaxRecord As Integer
    Public Property TxnCode As String
    Public Property Description As String
    Public Property Journalise As Integer
    Public Property Host As Integer
    Public Property IsReversal As Integer
    Public Property ReversalTxn As String
    Public Property TxnPath As Integer
    Public Property TxnIcon As Integer
    Public Property HostTxnCode As String
    Public Property TxnCode_Array As New List(Of String)
    Public Property Description_Array As New List(Of String)
    Public Property Journalise_Array As New List(Of Integer)
    Public Property Host_Array As New List(Of Integer)
    Public Property IsReversal_Array As New List(Of Integer)
    Public Property ReversalTxn_Array As New List(Of String)
    Public Property TxnPath_Array As New List(Of Integer)
    Public Property TxnIcon_Array As New List(Of Integer)
    Public Property HostTxnCode_Array As New List(Of String)
End Class
