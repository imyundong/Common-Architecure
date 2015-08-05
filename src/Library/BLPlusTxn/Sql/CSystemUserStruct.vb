''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : CSystemUserStruct.vb
' Class         : CSystemUserStruct
' Description   : CSystemUser -> Database Type
' Author        : Su Jia
' Creation Ver  : 2.1.0.0
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Version       Date                 User
' $Log: CSystemUserStruct.vb,v $
' Revision 1.0  2012/08/24           Su Jia
' Initial Version
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CSystemUserStruct
    Public Property MaxRecord As Integer
    Public Property UserId As Integer
    Public Property UserAlias As String
    Public Property Email As String
    Public Property FirstName As String
    Public Property LastName As String
    Public Property MiddleName As String
    Public Property FullName As String
    Public Property Gender As Short
    Public Property RegisterDate As DateTime
    Public Property LastLoginDate As DateTime
    Public Property Branch As Integer
    Public Property Photo As String
    Public Property PinBlock As String
    Public Property ContactNo As String
    Public Property ContactNo2 As String
    Public Property Nationality As String
    Public Property UserId_Array As New List(Of Integer)
    Public Property UserAlias_Array As New List(Of String)
    Public Property Email_Array As New List(Of String)
    Public Property FirstName_Array As New List(Of String)
    Public Property LastName_Array As New List(Of String)
    Public Property MiddleName_Array As New List(Of String)
    Public Property FullName_Array As New List(Of String)
    Public Property Gender_Array As New List(Of Short)
    Public Property RegisterDate_Array As New List(Of DateTime)
    Public Property LastLoginDate_Array As New List(Of DateTime)
    Public Property Branch_Array As New List(Of Integer)
    Public Property Photo_Array As New List(Of String)
    Public Property PinBlock_Array As New List(Of String)
    Public Property ContactNo_Array As New List(Of String)
    Public Property ContactNo2_Array As New List(Of String)
    Public Property Nationality_Array As New List(Of String)
End Class
