''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CMultiTxnSeqStruct.vb
' Class         : CMultiTxnSeqStruct
' Description   : CMultiTxnSeq -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CMultiTxnSeqStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CMultiTxnSeqStruct
    Public Property MaxRecord As Integer
    Public Property BANKCD As String
    Public Property BRANCHCD As String
    Public Property BUSINESSDATE As DateTime
    Public Property MULTISEQ As Integer
    Public Property EJSEQ As Integer
    Public Property DRAMT As Double
    Public Property CRAMT As Double
    Public Property TXNCD As String
    Public Property USERID As String
    Public Property TOACCTNO As String
    Public Property BANKCD_Array As New List(Of String)
    Public Property BRANCHCD_Array As New List(Of String)
    Public Property BUSINESSDATE_Array As New List(Of DateTime)
    Public Property MULTISEQ_Array As New List(Of Integer)
    Public Property EJSEQ_Array As New List(Of Integer)
    Public Property DRAMT_Array As New List(Of Double)
    Public Property CRAMT_Array As New List(Of Double)
    Public Property TXNCD_Array As New List(Of String)
    Public Property USERID_Array As New List(Of String)
    Public Property TOACCTNO_Array As New List(Of String)
End Class
