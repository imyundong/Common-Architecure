''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CBGLACCOUNTQUOTAPRStruct.vb
' Class         : CBGLACCOUNTQUOTAPRStruct
' Description   : CBGLACCOUNTQUOTAPR -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CBGLACCOUNTQUOTAPRStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CBGLACCOUNTQUOTAPRStruct
    Public Property MaxRecord As Integer
    Public Property TITLEID As String
    Public Property LIMITAMOUNT As Integer
    Public Property TITLEID_Array As New List(Of String)
    Public Property LIMITAMOUNT_Array As New List(Of Integer)
End Class
