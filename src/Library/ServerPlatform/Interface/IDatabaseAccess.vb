''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' File          : IDatabaseAccess.vb
' Class         : IDatabaseAccess
' Description   : Database Access Interface
' Author        : Su Jia
' Creation Date : 2012-12-13
'
' Revision History
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Date        Version  Author      Description
' 2012-08-21  0.1      Su Jia      Initial Draft
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Namespace Utility

    Public Interface IDatabaseAccess
        ' Query By Primary Key
        Sub Search(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object, ByVal Lock As Boolean?)
        Sub SearchAll(ByVal Adapter As IDatabaseAdapter, ByRef DatabaseObject As List(Of Object))
        Function Count(ByVal Adapter As IDatabaseAdapter) As Integer
        Sub Update(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object)
        Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object)
        Sub Remove(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object)
    End Interface
End Namespace