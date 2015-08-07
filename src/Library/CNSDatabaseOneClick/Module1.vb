Module Module1
    Private TableList As New List(Of CTable)
    Sub Main()
        ' Load All Files
        Console.WriteLine("1 - Load Table Definition")
        If LoadAllFiles() = False Then
            Console.WriteLine("Fail")
            Console.ReadKey()
            Console.ReadKey()
            Exit Sub
        End If

        Console.WriteLine("O.K.")
        Console.WriteLine("2 - Generate SQL")
        If GenerateSQL() = False Then
            Console.WriteLine("Fail")
            Console.ReadKey()
            Exit Sub
        End If

        Console.WriteLine("O.K.")
        Console.WriteLine("3 - Generate Database Class")
        If GenerateDatabaseClass() = False Then
            Console.WriteLine("Fail")
            Console.ReadKey()
            Exit Sub
        End If
        Console.WriteLine("O.K.")
        Console.WriteLine("4 - Generate Data Struct Class")
        If GenerateDatabaseStructClass() = False Then
            Console.WriteLine("Fail")
            Console.ReadKey()
            Exit Sub
        End If
        Console.WriteLine("O.K.")
        Console.WriteLine("5 - Generate SQL Class")
        If GenerateSQLClass() = False Then
            Console.WriteLine("Fail")
            Console.ReadKey()
            Exit Sub
        End If
        Console.WriteLine("O.K.")
        Console.ReadKey()
    End Sub

    Private Function LoadAllFiles() As Boolean
        Dim FileList As String() = IO.Directory.GetFiles(My.Application.Info.DirectoryPath, "*.def")
        If FileList.Length = 0 Then
            Console.WriteLine("Def File Not Found In {0}", My.Application.Info.DirectoryPath)
            Return False
        End If

        For Each File As String In FileList
            Dim Table As New CTable
            If LoadTable(File, Table) = False Then
                Return False
            End If
            TableList.Add(Table)
        Next

        Return True
    End Function

    Private Function LoadTable(ByVal File As String, ByRef Table As CTable) As Boolean
        If Not IO.File.Exists(File) Then
            Console.WriteLine("File Not Exist {0}", File)
            Return False
        End If

        Try
            Using Sr As New IO.StreamReader(File)
                Dim LineNumber As Integer = 0
                Dim Line As String = ""

                Dim CurrentSection As Section = Section.TABLE
                Dim CurrentQuery As New CQuery
                Dim CurrentCount As New CCount
                While Not Line Is Nothing
                    Line = Sr.ReadLine()

                    LineNumber += 1

                    If String.IsNullOrWhiteSpace(Line) OrElse Left(Line, 1) = "#" Then
                        Continue While
                    End If

                    If CurrentSection = Section.UNKNOWN Then
                        Dim TempStr As String() = Line.Split({" "}, StringSplitOptions.RemoveEmptyEntries)
                        If TempStr.Length <> 2 Then
                            Console.WriteLine("Invalid Section {0} @ Line {1}", Line, LineNumber)
                            Return False
                        End If

                        If TempStr(0).ToUpper = "QUERY" Then
                            CurrentSection = Section.QUERY
                            CurrentQuery = New CQuery
                            CurrentQuery.Id = TempStr(1)
                            Console.WriteLine("Query {0}", CurrentQuery.Id)
                            Continue While
                        ElseIf TempStr(0).ToUpper = "COUNT" Then
                            CurrentSection = Section.COUNT
                            CurrentCount = New CCount
                            CurrentCount.Id = TempStr(1)
                            Console.WriteLine("Count {0}", CurrentCount.Id)
                            Continue While
                        Else
                            Console.WriteLine("Invalid Section {0} @ Line {1}", Line, LineNumber)
                            Return False
                        End If
                    End If

                    If CurrentSection = Section.QUERY Then
                        If Line = "End Query" Then
                            If CurrentQuery.Queries.Count = 0 And CurrentQuery.Orders.Count = 0 Then
                                Console.WriteLine(" Query Items Not Found @ Line {0}", LineNumber)
                                Return False
                            Else
                                Table.Query.Add(CurrentQuery)
                                CurrentQuery = New CQuery
                                CurrentSection = Section.UNKNOWN
                                Continue While
                            End If
                        End If

                        If Line.Length >= 5 Then
                            Dim TempStr() As String = Line.Split({"=", " "}, StringSplitOptions.RemoveEmptyEntries)
                            If TempStr.Length < 2 Then
                                Console.WriteLine("Invalid Query Item Definition @ Line {0}", LineNumber)
                                Return False
                            End If

                            Dim Content As String = Line.Substring(Line.IndexOf("="c) + 1, Line.Length - Line.IndexOf("="c) - 1).Trim
                            If TempStr(0).ToUpper = "KEY" Then
                                Dim ItemList As New List(Of CKeyItem)
                                Dim Items As String() = Content.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                For Each Item As String In Items
                                    Dim Details As String() = Item.Split({"`"}, StringSplitOptions.RemoveEmptyEntries)
                                    If Details.Length > 5 Then
                                        Console.WriteLine("Invalid Query Item Definition {0} @ Line {1}", Item, LineNumber)
                                        Return False
                                    End If

                                    Dim KeyItem As New CKeyItem
                                    If Table.IsValidFiled(Details(0)) Then
                                        KeyItem.Field = Table.GetField(Details(0))
                                    Else
                                        Console.WriteLine("Invalid Query {0} @ Line {1}", Details(0), LineNumber)
                                        Return False
                                    End If

                                    If Details.Length >= 2 Then
                                        KeyItem.Oper = Details(1)
                                        If KeyItem.Oper = CEnviorment.INVALID Then
                                            Console.WriteLine("Invalid Operator {0} @ Line {1}", Details(1), LineNumber)
                                            Return False
                                        End If
                                    Else
                                        KeyItem.Oper = "="
                                    End If

                                    If Details.Length >= 3 Then
                                        KeyItem.Relation = Details(2)
                                        If KeyItem.Relation = CEnviorment.INVALID Then
                                            Console.WriteLine("Invalid Relation {0} @ Line {1}", Details(2), LineNumber)
                                            Return False
                                        End If
                                    End If

                                    If Details.Length = 4 Then
                                        If CKeyItem.SupportMethod.Contains(Details(3)) Then
                                            KeyItem.Method = Details(3)
                                        Else
                                            Console.WriteLine("Invalid Method {0} @ Line {1}", Details(3), LineNumber)
                                            Return False
                                        End If
                                    End If

                                    If Details.Length = 5 Then
                                        KeyItem.InternalMethod = Details(4)
                                        KeyItem.Method = Details(3)
                                    End If

                                    ItemList.Add(KeyItem)
                                Next

                                For Each Item As CKeyItem In ItemList
                                    Dim Count As Integer = ItemList.Where(Function(KeyItem) KeyItem.Field.Id = Item.Field.Id).Count
                                    Item.Repeat = Count
                                Next

                                CurrentQuery.Queries.Add(ItemList)

                                ' ElseIf TempStr(0).ToUpper = "ORDER" Then
                            ElseIf TempStr(0).ToUpper = "ORDER" Then

                                Dim Items As String() = Content.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                If CurrentQuery.Orders.Count > 0 Then
                                    Console.WriteLine("Duplicate Order Defined @ Line {0}", LineNumber)
                                    Return False
                                End If

                                For Each Item As String In Items
                                    Dim Details As String() = Item.Split({"`"}, StringSplitOptions.RemoveEmptyEntries)
                                    If Details.Length > 2 Then
                                        Console.WriteLine("Invalid Order Item Definition {0} @ Line {1}", Item, LineNumber)
                                        Return False
                                    End If

                                    Dim OrderItem As New COrderItem
                                    If Table.IsValidFiled(Details(0)) Then
                                        OrderItem.Field = Table.GetField(Details(0))
                                    Else
                                        Console.WriteLine("Invalid Order {0} @ Line {1}", Details(0), LineNumber)
                                        Return False
                                    End If

                                    If Details.Length = 2 Then
                                        OrderItem.Order = Details(1)
                                        If OrderItem.Order = CEnviorment.INVALID Then
                                            Console.WriteLine("Invalid Order Type {0} @ Line {1}", Details(1), LineNumber)
                                            Return False
                                        End If
                                    End If

                                    CurrentQuery.Orders.Add(OrderItem)

                                Next

                            Else
                                Console.WriteLine("Invalid Query Item Definition @ Line {0}", LineNumber)
                                Return False
                            End If
                        End If
                    End If

                    ' Key   = TEST_DATE`>=`TODATETIME
                    If CurrentSection = Section.COUNT Then

                        If Line = "End Count" Then
                            If CurrentCount.CountItems.Count = 0 Then
                                Console.WriteLine("Count Items Not Found @ Line {0}", LineNumber)
                                Return False
                            Else
                                Table.Count.Add(CurrentCount)
                                CurrentCount = New CCount
                            End If
                            CurrentSection = Section.UNKNOWN
                            Continue While
                        End If

                        If Line.Length >= 5 Then
                            Dim TempStr() As String = Line.Split({"=", " "}, StringSplitOptions.RemoveEmptyEntries)
                            If TempStr.Length < 2 Then
                                Console.WriteLine("Invalid Count Item Definition @ Line {0}", LineNumber)
                                Return False
                            End If


                            Dim Content As String = Line.Substring(Line.IndexOf("="c) + 1, Line.Length - Line.IndexOf("="c) - 1).Trim
                            If TempStr(0).ToUpper = "KEY" Then
                                Dim ItemList As New List(Of CKeyItem)
                                Dim Items As String() = Content.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                For Each Item As String In Items
                                    Dim Details As String() = Item.Split({"`"}, StringSplitOptions.RemoveEmptyEntries)
                                    If Details.Length > 5 Then
                                        Console.WriteLine("Invalid Count Item Definition {0} @ Line {1}", Item, LineNumber)
                                        Return False
                                    End If

                                    Dim KeyItem As New CKeyItem
                                    If Table.IsValidFiled(Details(0)) Then
                                        KeyItem.Field = Table.GetField(Details(0))
                                    Else
                                        Console.WriteLine("Invalid Fields {0} @ Line {1}", Details(0), LineNumber)
                                        Return False
                                    End If

                                    If Details.Length >= 2 Then
                                        KeyItem.Oper = Details(1)
                                        If KeyItem.Oper = CEnviorment.INVALID Then
                                            Console.WriteLine("Invalid Operator {0} @ Line {1}", Details(1), LineNumber)
                                            Return False
                                        End If
                                    Else
                                        KeyItem.Oper = "="
                                    End If

                                    If Details.Length >= 3 Then
                                        KeyItem.Relation = Details(2)
                                        If KeyItem.Relation = CEnviorment.INVALID Then
                                            Console.WriteLine("Invalid Relation {0} @ Line {1}", Details(2), LineNumber)
                                            Return False
                                        End If
                                    Else
                                        KeyItem.Oper = "="
                                    End If

                                    If Details.Length = 4 Then
                                        If CKeyItem.SupportMethod.Contains(Details(3)) Then
                                            KeyItem.Method = Details(3)
                                        Else
                                            Console.WriteLine("Invalid Method {0} @ Line {1}", Details(3), LineNumber)
                                            Return False
                                        End If
                                    End If

                                    If Details.Length = 5 Then
                                        KeyItem.InternalMethod = Details(4)
                                        KeyItem.Method = Details(3)
                                    End If
                                    ItemList.Add(KeyItem)
                                Next
                                CurrentCount.CountItems.Add(ItemList)

                            Else
                                Console.WriteLine("Invalid Count Item Definition @ Line {0}", LineNumber)
                                Return False
                            End If
                        Else
                            Console.WriteLine("Invalid Count Item Definition @ Line {0}", LineNumber)
                            Return False
                        End If

                    End If

                    ' Table DBTest
                    ' Y Y TEST_ID           INT      4    0
                    ' N N TEST_VALUE        VARCHAR  MAX  ""
                    ' N N TEST_DATE         DATETIME 8    Now()
                    ' End Table
                    If CurrentSection = Section.TABLE Then
                        If Table.Id = "" Then
                            If Line.Length <= 8 Then
                                Console.WriteLine("Invalid Table Definition @ Line " & LineNumber)
                                Return False
                            Else
                                Dim TempStr As String() = Line.Split({" "}, StringSplitOptions.RemoveEmptyEntries)
                                If TempStr.Length <> 2 Then
                                    Console.WriteLine("Invalid Table Definition @ Line " & LineNumber)
                                    Return False
                                ElseIf TempStr(0).ToUpper <> "TABLE" Then
                                    Console.WriteLine("Invalid Table Definition @ Line " & LineNumber)
                                    Return False
                                Else
                                    Dim TempStr2 As String() = TempStr(1).Split("<")
                                    Table.Id = TempStr2(0)
                                    For i As Integer = 1 To TempStr2.Length - 1
                                        Table.Attr.Add("<" + TempStr2(i))
                                    Next

                                    Console.WriteLine("Table {0}", Table.Id)
                                End If
                            End If
                        Else
                            Dim TempStr As String() = Line.Split({" "}, StringSplitOptions.RemoveEmptyEntries)
                            If TempStr.Length < 5 Then
                                If Line <> "End Table" Then
                                    Console.WriteLine("Invalid Field Definition @ Line " & LineNumber)
                                    Return False
                                Else
                                    If Table.Fields.Count = 0 Then
                                        Console.WriteLine("Fields Not Defined")
                                        Return False
                                    End If
                                    Console.WriteLine("Table Define Finished")
                                    CurrentSection = Section.UNKNOWN
                                    Continue While
                                End If
                            End If

                            Dim Field As New CField
                            If TempStr(0) = "N" Then
                                Field.Key = False
                            ElseIf TempStr(0) = "Y" Then
                                Field.Key = True
                            Else
                                Console.WriteLine("Invalid Field Definition @ Line " & LineNumber)
                                Return False
                            End If

                            If TempStr(1) = "N" Then
                                Field.NotNull = False
                            ElseIf TempStr(0) = "Y" Then
                                Field.NotNull = True
                            Else
                                Console.WriteLine("Invalid Field Definition @ Line " & LineNumber)
                                Return False
                            End If

                            Field.Id = TempStr(2)
                            Field.DataType = TempStr(3)
                            If Field.DataType = CEnviorment.INVALID Then
                                Console.WriteLine("Unknown Data Type {0} @ Line {1}", TempStr(3), LineNumber)
                                Return False
                            End If

                            If Field.DataType = "VARCHAR" And TempStr(4) = "MAX" Then
                                Field.Length = "MAX"
                            Else
                                Dim IntegerLength As Integer
                                If Integer.TryParse(TempStr(4), IntegerLength) = False Then
                                    Console.WriteLine("Invalid Field Length {0} @ Line {1}", TempStr(4), LineNumber)
                                    Return False
                                End If
                                Field.Length = IntegerLength
                            End If

                            If Field.DataType = "VARCHAR" Then
                                If Field.Length <> "MAX" Then
                                    If Field.Length < 2 Then
                                        Console.WriteLine("Invalid Field Length {0} @ Line {1}", TempStr(4), LineNumber)
                                        Return False
                                    End If
                                End If

                                If TempStr.Length = 6 Then
                                    Field.Value = Chr(34) & TempStr(5).Trim(Chr(34)) & Chr(34)
                                Else
                                    Field.Value = Chr(34) & Chr(34)
                                End If
                            ElseIf Field.DataType = "CHAR" Then
                                If Field.Length < 1 Then
                                    Console.WriteLine("Invalid Field Length {0} @ Line {1}", TempStr(4), LineNumber)
                                    Return False
                                End If

                                If TempStr.Length = 6 Then
                                    If TempStr(5).Trim(Chr(34)).Length > Field.Length Then
                                        Console.WriteLine("Invalid Default Value Length {0} @ Line {1}", TempStr(5), LineNumber)
                                        Return False
                                    End If
                                    Field.Value = TempStr(5).Trim(Chr(34))
                                    If Field.Value.Length < Field.Length Then
                                        Field.Value = Field.Value & New String(" ", Field.Length - Field.Value.Length)
                                        Field.Value = Chr(34) & Field.Value & Chr(34)
                                    End If
                                Else
                                    Field.Value = Chr(34) & New String(" ", File.Length) & Chr(34)
                                End If

                            ElseIf Field.DataType = "DATETIME" Then
                                If Field.Length <> 8 And Field.Length <> 4 Then
                                    Console.WriteLine("Invalid Field Length {0} @ Line {1}", TempStr(5), LineNumber)
                                    Return False
                                End If

                                If TempStr.Length = 6 Then
                                    Field.Value = TempStr(5)
                                Else
                                    Field.Value = "CEnviorment.INVALID_DATETIME"
                                End If
                            ElseIf Field.DataType = "INT" Then
                                If Field.Length <> 2 And Field.Length <> 4 And Field.Length <> 8 Then
                                    Console.WriteLine("Invalid Field Length {0} @ Line {1}", TempStr(4), LineNumber)
                                    Return False
                                End If

                                If TempStr.Length = 6 Then
                                    Dim TempShort As Short
                                    If Short.TryParse(TempStr(5), TempShort) = False Then
                                        Console.WriteLine("Invalid Default Value {0} @ Line {1}", TempStr(5), LineNumber)
                                        Return False
                                    End If
                                    Field.Value = TempShort
                                Else
                                    Field.Value = "0"
                                End If
                            ElseIf Field.DataType = "FLOAT" Then
                                If Field.Length <> 4 And Field.Length <> 8 Then
                                    Console.WriteLine("Invalid Field Length {0} @ Line {1}", TempStr(4), LineNumber)
                                    Return False
                                End If

                                If TempStr.Length = 6 Then
                                    Dim TempShort As Decimal
                                    If Decimal.TryParse(TempStr(5), TempShort) = False Then
                                        Console.WriteLine("Invalid Default Value {0} @ Line {1}", TempStr(5), LineNumber)
                                        Return False
                                    End If
                                    Field.Value = TempShort
                                Else
                                    Field.Value = "0"
                                End If
                            End If

                            Table.Fields.Add(Field)
                            Console.WriteLine("Field {0}, {1}, {2}, {3}, {4}, {5}", Field.Id, Field.Key, Field.NotNull, Field.DataType, Field.Length, Field.Value)


                        End If
                    End If
                End While

            End Using

            Return True
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return False
        End Try

        Return True
    End Function

    Private Function GenerateDatabaseStructClass() As Boolean
        For Each Table As CTable In TableList
            If GenerateDatabaseStructClass(Table) = False Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Function GenerateDatabaseStructClass(ByVal Table As CTable) As Boolean
        Console.WriteLine("Generate " & "C" & Table.Id & "Struct.vb")
        Try
            Using Sw As New IO.StreamWriter("C" & Table.Id & "Struct.vb")

                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("' File          : C" & Table.Id & "Struct.vb")
                Sw.WriteLine("' Class         : C" & Table.Id & "Struct")
                Sw.WriteLine("' Description   : C" & Table.Id & " -> Database Type")
                Sw.WriteLine("' Author        : Su Jia")
                Sw.WriteLine("' Creation Ver  : " + My.Application.Info.Version.ToString)
                Sw.WriteLine("'")
                Sw.WriteLine("' Revision History")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("' Version       Date                 User")
                Sw.WriteLine("' $Log: C" & Table.Id & "Struct.vb,v $")
                Sw.WriteLine("' Revision 1.0  2012/08/24           Su Jia")
                Sw.WriteLine("' Initial Version")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                'Sw.WriteLine("Imports CNSCore")
                Sw.WriteLine("Public Class C" & Table.Id & "Struct")
                'Sw.WriteLine("    Implements IDatabaseStruct")
                Sw.WriteLine("    Public Property MaxRecord As Integer")
                For Each Field As CField In Table.Fields
                    Sw.WriteLine("    Public Property " & Field.Id & " As " & Field.GetInternalDataType)
                Next

                For Each Field As CField In Table.Fields
                    Sw.WriteLine("    Public Property " & Field.Id & "_Array As New List(Of " & Field.GetInternalDataType & ")")
                Next

                Sw.WriteLine("End Class")

            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function GenerateSQLClass() As Boolean
        For Each Table As CTable In TableList
            If GenerateSQLClass(Table, "SQLServer") = False Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function GenerateSQLClass(ByVal Table As CTable, ByVal DatabaseType As String) As Boolean
        Console.WriteLine(DatabaseType & IO.Path.DirectorySeparatorChar & "C" & Table.Id & DatabaseType & ".vb")

        Try
            If Not IO.Directory.Exists(DatabaseType) Then
                IO.Directory.CreateDirectory(DatabaseType)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        Try
            Using Sw As New IO.StreamWriter(DatabaseType & IO.Path.DirectorySeparatorChar & "C" & Table.Id & DatabaseType & ".vb")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("' File          : C" & Table.Id & ".vb")
                Sw.WriteLine("' Class         : C" & Table.Id & "")
                Sw.WriteLine("' Description   : Table    " & Table.Id & "")
                Sw.WriteLine("'               : Database SQLServer")
                Sw.WriteLine("'               : This Class is Generated By DBCG Tools Automatically")
                Sw.WriteLine("' Author        : Su Jia")
                Sw.WriteLine("' Creation Ver  : " + My.Application.Info.Version.ToString)
                Sw.WriteLine("'")
                Sw.WriteLine("' Revision History")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("' Version       Date                 User")
                Sw.WriteLine("' $Log: C" & Table.Id & ".vb,v $")
                Sw.WriteLine("' Revision 1.0  2012/08/24           Su Jia")
                Sw.WriteLine("' Initial Version")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("Imports ServerPlatform.Library.Utility")
                Sw.WriteLine("Imports ServerPlatform.Library.Workflow.CError")
                Sw.WriteLine("")
                Sw.WriteLine("Public Class C" & Table.Id & "SQLServer")
                Sw.WriteLine("    Inherits C" & Table.Id & "Struct")
                Sw.WriteLine("    Implements IDatabaseAccess")
                Sw.WriteLine("    Implements IComponent")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Insert(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _")
                Sw.WriteLine("        Implements IDatabaseAccess.Insert")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObj As C" & Table.Id & "Struct = TryCast(Obj, C" & Table.Id & "Struct)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " INSERT INTO " & Table.Id & " " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & "    ( " & Chr(34) & ")")

                For Idx As Integer = 0 To Table.Fields.Count - 1
                    If Idx = Table.Fields.Count - 1 Then
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "]) " & Chr(34) & ")")
                    Else
                        If Table.Fields(Idx).DataType <> "IDENTITY" Then Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "], " & Chr(34) & ")")
                    End If
                Next

                Sw.WriteLine("        SQL.Append(" & Chr(34) & " VALUES " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & "    ( " & Chr(34) & ")")

                For Idx As Integer = 0 To Table.Fields.Count - 1
                    If Idx = Table.Fields.Count - 1 Then
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "    @" & Table.Fields(Idx).Id & ") " & Chr(34) & ")")
                    Else
                        If Table.Fields(Idx).DataType <> "IDENTITY" Then Sw.WriteLine("        SQL.Append(" & Chr(34) & "    @" & Table.Fields(Idx).Id & ", " & Chr(34) & ")")
                    End If
                Next

                Sw.WriteLine("")
                Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command()")
                Sw.WriteLine("")
                Sw.WriteLine("        Command.CommandType = CommandType.Text")
                Sw.WriteLine("        Command.CommandText = SQL.ToString")
                For Idx As Integer = 0 To Table.Fields.Count - 1
                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & Table.Fields(Idx).Id & Chr(34) & ", StructObj." & Table.Fields(Idx).Id & ")")
                Next

                Sw.WriteLine("")
                Sw.WriteLine("        Try")
                Sw.WriteLine("            Dim Counter As Integer = Command.ExecuteNonQuery()")
                Sw.WriteLine("            If Counter <> 1 Then")
                Sw.WriteLine("                Throw New CBusinessException(CErrorCode.DATABASE_INSERT_FAIL, ""0 Record Inserted"")")
                Sw.WriteLine("            End If")

                If Table.Fields(0).DataType = "IDENTITY" Then
                    Sw.WriteLine("         Command = Adapter.Command()")
                    Sw.WriteLine("         Command.CommandText = " & Chr(34) & "SELECT @@Identity FROM " & Table.Id & Chr(34))
                    Sw.WriteLine("         Dim Identity As Integer = Command.ExecuteScalar()")
                    Sw.WriteLine("         StructObj." + Table.Fields(0).Id + " = Identity")
                End If
                Sw.WriteLine("        Catch ex As Exception")
                Sw.WriteLine("            CLog.Err(ex.Message)")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_INSERT_FAIL, ex)")
                Sw.WriteLine("        End Try")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Update(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _")
                Sw.WriteLine("        Implements IDatabaseAccess.Update")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObj As C" & Table.Id & "Struct = TryCast(Obj, C" & Table.Id & "Struct)")
                Sw.WriteLine("")

                Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " UPDATE " & Table.Id & " " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " SET " & Chr(34) & ")")

                For Idx As Integer = 0 To Table.GetFieldsWithoutPK.Count - 1
                    If Idx = Table.GetFieldsWithoutPK.Count - 1 Then
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     [" & Table.GetFieldsWithoutPK(Idx).Id & "] = @" & Table.GetFieldsWithoutPK(Idx).Id & " " & Chr(34) & ")")
                    Else
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     [" & Table.GetFieldsWithoutPK(Idx).Id & "] = @" & Table.GetFieldsWithoutPK(Idx).Id & ", " & Chr(34) & ")")
                    End If
                Next

                Sw.WriteLine("        SQL.Append(" & Chr(34) & " WHERE " & Chr(34) & ")")

                For Idx As Integer = 0 To Table.GetPrimaryKey.Count - 1
                    Dim Rel As String = "   "
                    If Idx >= 1 Then Rel = "AND"
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " " & Rel & " [" & Table.GetPrimaryKey(Idx).Id & "] = @" & Table.GetPrimaryKey(Idx).Id & " " & Chr(34) & ")")
                Next

                Sw.WriteLine("")
                Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command()")
                Sw.WriteLine("")
                Sw.WriteLine("        Command.CommandType = CommandType.Text")
                Sw.WriteLine("        Command.CommandText = SQL.ToString")
                For Idx As Integer = 0 To Table.Fields.Count - 1
                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & Table.Fields(Idx).Id & Chr(34) & ", StructObj." & Table.Fields(Idx).Id & ")")
                Next
                Sw.WriteLine("")
                Sw.WriteLine("        Try")
                Sw.WriteLine("            Dim Counter As Integer = Command.ExecuteNonQuery()")
                Sw.WriteLine("            If Counter <> 1 Then")
                Sw.WriteLine("                Throw New CBusinessException(CErrorCode.DATABASE_UPDATE_FAIL, ""0 Record Updated"")")
                Sw.WriteLine("            End If")
                Sw.WriteLine("        Catch ex As Exception")
                Sw.WriteLine("            CLog.Err(ex.Message)")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_UPDATE_FAIL, ex)")
                Sw.WriteLine("        End Try")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Remove(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) _")
                Sw.WriteLine("        Implements IDatabaseAccess.Remove")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObj As C" & Table.Id & "Struct = TryCast(Obj, C" & Table.Id & "Struct)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " DELETE FROM " & Table.Id & " " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " WHERE " & Chr(34) & ")")
                For Idx As Integer = 0 To Table.GetPrimaryKey.Count - 1
                    Dim Rel As String = "   "
                    If Idx >= 1 Then Rel = "AND"
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " " & Rel & " [" & Table.GetPrimaryKey(Idx).Id & "] = @" & Table.GetPrimaryKey(Idx).Id & " " & Chr(34) & ")")
                Next

                Sw.WriteLine("")
                Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command")
                Sw.WriteLine("")
                Sw.WriteLine("        Command.CommandType = CommandType.Text")
                Sw.WriteLine("        Command.CommandText = SQL.ToString")
                For Idx As Integer = 0 To Table.GetPrimaryKey.Count - 1
                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & Table.GetPrimaryKey(Idx).Id & Chr(34) & ", StructObj." & Table.GetPrimaryKey(Idx).Id & ")")
                Next
                Sw.WriteLine("")
                Sw.WriteLine("        Try")
                Sw.WriteLine("            Dim Counter As Integer = Command.ExecuteNonQuery()")
                Sw.WriteLine("            If Counter <> 1 Then")
                Sw.WriteLine("                Throw New CBusinessException(CErrorCode.DATABASE_UPDATE_FAIL, ""0 Record Deleted"")")
                Sw.WriteLine("            End If")
                Sw.WriteLine("        Catch ex As Exception")
                Sw.WriteLine("            CLog.Err(ex.Message)")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_DELETE_FAIL, ex)")
                Sw.WriteLine("        End Try")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Search(ByVal Adapter As IDatabaseAdapter, _")
                Sw.WriteLine("                           ByRef Obj As Object, ByVal Lock As Boolean?) Implements IDatabaseAccess.Search")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObj As C" & Table.Id & "Struct = TryCast(Obj, C" & Table.Id & "Struct)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " SELECT " & Chr(34) & ")")

                For Idx As Integer = 0 To Table.Fields.Count - 1
                    If Idx = Table.Fields.Count - 1 Then
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "] " & Chr(34) & ")")
                    Else
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "], " & Chr(34) & ")")
                    End If
                Next
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " FROM " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & Table.Id & " " & Chr(34) & ")")
                Sw.WriteLine("        If Lock Is Nothing Then")
                Sw.WriteLine("            SQL.Append(" & Chr(34) & " WITH (NOLOCK) " & Chr(34) & ")")
                Sw.WriteLine("        ElseIf Lock = True Then")
                Sw.WriteLine("            SQL.Append(" & Chr(34) & " WITH (ROWLOCK) " & Chr(34) & ")")
                Sw.WriteLine("        End If")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " WHERE" & Chr(34) & ")")
                For Idx As Integer = 0 To Table.GetPrimaryKey.Count - 1
                    Dim Rel As String = "   "
                    If Idx >= 1 Then Rel = "AND"
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " " & Rel & " [" & Table.GetPrimaryKey(Idx).Id & "] = @" & Table.GetPrimaryKey(Idx).Id & " " & Chr(34) & ")")
                Next
                Sw.WriteLine("")
                Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command")
                Sw.WriteLine("")
                Sw.WriteLine("        Command.CommandType = CommandType.Text")
                Sw.WriteLine("        Command.CommandText = SQL.ToString")
                For Idx As Integer = 0 To Table.GetPrimaryKey.Count - 1
                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & Table.GetPrimaryKey(Idx).Id & Chr(34) & ", StructObj." & Table.GetPrimaryKey(Idx).Id & ")")
                Next
                Sw.WriteLine("")
                Sw.WriteLine("        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter")
                Sw.WriteLine("        SQLAdapter.SelectCommand = Command")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim Data As New DataSet")
                Sw.WriteLine("        Try")
                Sw.WriteLine("            SQLAdapter.Fill(Data)")
                Sw.WriteLine("        Catch ex As Exception")
                Sw.WriteLine("            CLog.Err(ex.Message)")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)")
                Sw.WriteLine("        End Try")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim Row As Data.DataRowCollection = Data.Tables(0).Rows")
                Sw.WriteLine("        If Row.Count = 0 Then")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, ""Record Not Found"")")
                Sw.WriteLine("        End If")
                Sw.WriteLine("")

                For Index As Integer = 0 To Table.Fields.Count - 1
                    Sw.WriteLine("        If Not IsDBNull(Row(0).Item(" & Index & ")) Then StructObj." & Table.Fields(Index).Id & " = Row(0).Item(" & Index & ")")
                Next

                Sw.WriteLine("    End Sub")
                ' Search ALL
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub SearchAll(ByVal Adapter As IDatabaseAdapter, _")
                Sw.WriteLine("                         ByRef DatabaseObj As List(Of Object)) Implements IDatabaseAccess.SearchAll")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " SELECT " & Chr(34) & ")")

                For Idx As Integer = 0 To Table.Fields.Count - 1
                    If Idx = Table.Fields.Count - 1 Then
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "] " & Chr(34) & ")")
                    Else
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "], " & Chr(34) & ")")
                    End If
                Next
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " FROM " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & Table.Id & " " & Chr(34) & ")")
                Sw.WriteLine("")

                Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command")
                Sw.WriteLine("")
                Sw.WriteLine("        Command.CommandType = CommandType.Text")
                Sw.WriteLine("        Command.CommandText = SQL.ToString")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter")
                Sw.WriteLine("        SQLAdapter.SelectCommand = Command")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim Data As New DataSet")
                Sw.WriteLine("        Try")
                Sw.WriteLine("            SQLAdapter.Fill(Data)")
                Sw.WriteLine("        Catch ex As Exception")
                Sw.WriteLine("            CLog.Err(ex.Message)")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)")
                Sw.WriteLine("        End Try")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim Rows As Data.DataRowCollection = Data.Tables(0).Rows")
                Sw.WriteLine("        If Rows.Count = 0 Then")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, ""Record Not Found"")")
                Sw.WriteLine("        End If")
                Sw.WriteLine("")
                Sw.WriteLine("        For Each Row As DataRow In Rows")
                Sw.WriteLine("            Dim StructObj As New C" & Table.Id & "Struct")
                For Index As Integer = 0 To Table.Fields.Count - 1
                    Sw.WriteLine("            If Not IsDBNull(Row.Item(" & Index & ")) Then StructObj." & Table.Fields(Index).Id & " = Row.Item(" & Index & ")")
                Next
                Sw.WriteLine("            DatabaseObj.Add(StructObj)")
                Sw.WriteLine("        Next")
                Sw.WriteLine("    End Sub")

                For Each Query As CQuery In Table.Query
                    Sw.WriteLine("")
                    Sw.WriteLine("    Public Sub Search" & Query.Id & "(ByVal Adapter As IDatabaseAdapter, _")
                    Sw.WriteLine("                                      ByVal Obj As C" & Table.Id & "Struct, _")
                    Sw.WriteLine("                                      ByRef " & Table.Id & "List As List(Of Object), _")
                    Sw.WriteLine("                                      ByVal Lock As Boolean?) _")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim StructObj As C" & Table.Id & "Struct = TryCast(Obj, C" & Table.Id & "Struct)")
                    Sw.WriteLine("        " & Table.Id & "List.Clear()")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " SELECT " & Chr(34) & ")")
                    Sw.Write("        If StructObj.MaxRecord > 0 Then")
                    Sw.WriteLine(" SQL.Append(" & Chr(34) & " TOP " & Chr(34) & " & StructObj.MaxRecord)")

                    For Idx As Integer = 0 To Table.Fields.Count - 1
                        If Idx = Table.Fields.Count - 1 Then
                            Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "]  " & Chr(34) & ")")
                        Else
                            Sw.WriteLine("        SQL.Append(" & Chr(34) & "    [" & Table.Fields(Idx).Id & "], " & Chr(34) & ")")
                        End If
                    Next

                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " FROM " & Chr(34) & ")")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & Table.Id & " " & Chr(34) & ")")
                    Sw.WriteLine("        If Lock Is Nothing Then")
                    Sw.WriteLine("            SQL.Append(" & Chr(34) & " WITH (NOLOCK) " & Chr(34) & ")")
                    Sw.WriteLine("        ElseIf Lock = True Then")
                    Sw.WriteLine("            SQL.Append(" & Chr(34) & " WITH (ROWLOCK) " & Chr(34) & ")")
                    Sw.WriteLine("        End If")
                    If Query.Queries.Count <> 0 Then Sw.WriteLine("        SQL.Append(" & Chr(34) & " WHERE " & Chr(34) & ")")
                    Dim Counter As Integer = 0

                    For Each KeyList As List(Of CKeyItem) In Query.Queries
                        Counter += 1
                        If Counter > 1 Then
                            Sw.WriteLine("        SQL.Append(" & Chr(34) & "     AND " & Chr(34) & ")")
                        End If

                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     ( " & Chr(34) & ")")
                        Dim Index As Integer = 0
                        Dim PreRel As String = "AND"
                        For Each KeyItem As CKeyItem In KeyList
                            Index += 1

                            If Index > 1 Then
                                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & PreRel & " " & Chr(34) & ")")
                            End If

                            If KeyItem.Method <> "" Then

                                If KeyItem.InternalMethod = "" Then
                                    If KeyItem.Method = "TODATETIME" Then
                                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     CONVERT(varchar(14), [" & KeyItem.Field.Id & "], 100) " & KeyItem.Oper & " @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                    ElseIf KeyItem.Method = "TODATE" Then
                                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     CONVERT(varchar(8), [" & KeyItem.Field.Id & "], 112) " & KeyItem.Oper & " @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                    Else
                                        Return False
                                    End If
                                Else

                                    Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & KeyItem.Method & " " & KeyItem.Oper & " @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                End If
                            Else
                                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     [" & KeyItem.Field.Id & "] " & KeyItem.Oper & " " & Chr(34) & ")")
                                If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                    Sw.WriteLine("        SQL.Append(""     ("")")
                                    Sw.WriteLine("        Dim Idx1 As Integer")
                                    Sw.WriteLine("        For Idx1 = 1 To Obj." & KeyItem.Field.Id & "_Array.Count - 1")
                                    Sw.WriteLine("            SQL.Append(""       @" & KeyItem.Field.Id & "_"" + Idx1.ToString + "", "")")
                                    Sw.WriteLine("        Next")
                                    Sw.WriteLine("        SQL.Append(""       @" & KeyItem.Field.Id & "_"" + Idx1.ToString)")
                                    Sw.WriteLine("        SQL.Append(""     ) "")")
                                Else
                                    If KeyItem.Repeat >= 2 Then
                                        Sw.WriteLine("        SQL.Append(""       @" & KeyItem.Field.Id & Index.ToString & " " & Chr(34) & ")")
                                    Else
                                        Sw.WriteLine("        SQL.Append(""       @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                    End If

                                End If


                            End If
                            PreRel = KeyItem.Relation
                        Next

                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     ) " & Chr(34) & ")")
                    Next

                    Counter = 0
                    For Each Order As COrderItem In Query.Orders
                        If Counter = 0 Then
                            Sw.WriteLine("        SQL.Append(" & Chr(34) & " ORDER BY " & Chr(34) & ")")
                        End If
                        Counter += 1
                        If Counter > 1 Then
                            Sw.WriteLine("        SQL.Append(" & Chr(34) & "     , " & Chr(34) & ")")
                        End If
                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & Order.Field.Id & " " & Order.Order & " " & Chr(34) & ")")
                    Next

                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Command.CommandType = CommandType.Text")
                    Sw.WriteLine("        Command.CommandText = SQL.ToString")

                    For Each KeyList As List(Of CKeyItem) In Query.Queries
                        Counter = 0
                        Dim Position As Integer = 0
                        For Each KeyItem As CKeyItem In KeyList
                            Counter += 1
                            If KeyItem.Method <> "" Then
                                If KeyItem.InternalMethod = "" Then
                                    If KeyItem.Method = "TODATETIME" Then
                                        Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & ".ToString(" & Chr(34) & "yyyyMMddHHmmss" & Chr(34) & "))")
                                    ElseIf KeyItem.Method = "TODATE" Then
                                        Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & ".ToString(" & Chr(34) & "yyyyMMdd" & Chr(34) & "))")
                                    Else
                                        Return False
                                    End If
                                Else
                                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & "." & KeyItem.InternalMethod & ")")
                                End If
                            Else
                                If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                    Sw.WriteLine("        For Idx2 As Integer = 0 To Obj." & KeyItem.Field.Id & "_Array.Count - 1")
                                    Sw.WriteLine("            Command.Parameters.AddWithValue(""" & KeyItem.Field.Id & "_"" + (Idx2 + 1).ToString, StructObj." & KeyItem.Field.Id & "_Array(Idx2))")
                                    Sw.WriteLine("        Next")
                                Else
                                    If KeyItem.Repeat >= 2 Then

                                        Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id + Counter.ToString & Chr(34) & ", StructObj." & KeyItem.Field.Id & "_Array(" & Position.ToString & "))")
                                        Position += 1
                                    Else
                                        Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & ")")
                                    End If

                                End If
                            End If
                        Next
                    Next

                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim SQLAdapter As SqlClient.SqlDataAdapter = Adapter.Adapter")
                    Sw.WriteLine("        SQLAdapter.SelectCommand = Command")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim Data As New DataSet")
                    Sw.WriteLine("        Try")
                    Sw.WriteLine("            SQLAdapter.Fill(Data)")
                    Sw.WriteLine("        Catch ex As Exception")
                    Sw.WriteLine("            CLog.Err(ex.Message)")
                    Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)")
                    Sw.WriteLine("        End Try")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim Row As DataRowCollection = Data.Tables(0).Rows")
                    Sw.WriteLine("        If Row.Count = 0 Then")
                    Sw.WriteLine("            Throw New CBusinessException(CErrorCode.RECORD_NOT_FOUND, ""Record Not Found"")")
                    Sw.WriteLine("        End If")
                    Sw.WriteLine("")
                    Sw.WriteLine("        For Idx As Integer = 0 To Data.Tables(0).Rows.Count - 1")
                    Sw.WriteLine("            Dim DBStruct As New C" & Table.Id & "Struct")
                    Sw.WriteLine("")
                    For Index As Integer = 0 To Table.Fields.Count - 1
                        Sw.WriteLine("            If Not IsDBNull(Row(Idx).Item(" & Index & ")) Then DBStruct." & Table.Fields(Index).Id & " = Row(Idx).Item(" & Index & ")")
                    Next
                    Sw.WriteLine("")
                    Sw.WriteLine("            " & Table.Id & "List.Add(DBStruct)")
                    Sw.WriteLine("        Next")
                    Sw.WriteLine("")
                    Sw.WriteLine("    End Sub")
                Next
                Sw.WriteLine("")
                Sw.WriteLine("    Public Function Count(ByVal Adapter As IDatabaseAdapter) As Integer Implements IDatabaseAccess.Count")
                Sw.WriteLine("")

                Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " SELECT " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     COUNT(*) " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & " FROM " & Chr(34) & ")")
                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & Table.Id & " " & Chr(34) & ")")

                Sw.WriteLine("")
                Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command")
                Sw.WriteLine("")
                Sw.WriteLine("        Command.CommandType = CommandType.Text")
                Sw.WriteLine("        Command.CommandText = SQL.ToString")
                Sw.WriteLine("        ")
                Sw.WriteLine("        Try")
                Sw.WriteLine("            Dim Counter As Integer = Command.ExecuteScalar()")
                Sw.WriteLine("            Return Count")
                Sw.WriteLine("        Catch ex As Exception")
                Sw.WriteLine("            CLog.Err(ex.Message)")
                Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)")
                Sw.WriteLine("        End Try")
                Sw.WriteLine("        ")
                Sw.WriteLine("    End Function")

                For Each Count As CCount In Table.Count
                    Sw.WriteLine("")
                    Sw.WriteLine("    Public Function Count" & Count.Id & "(ByVal Adapter As IDatabaseAdapter, ByRef Obj As Object) As Integer")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim StructObj As C" & Table.Id & "Struct = TryCast(Obj, C" & Table.Id & "Struct)")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim SQL As New Text.StringBuilder")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " SELECT " & Chr(34) & ")")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & "     COUNT(*) " & Chr(34) & ")")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " FROM " & Chr(34) & ")")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & Table.Id & " " & Chr(34) & ")")
                    Sw.WriteLine("        SQL.Append(" & Chr(34) & " WHERE" & Chr(34) & ")")
                    Dim Counter As Integer = 0
                    For Each KeyList As List(Of CKeyItem) In Count.CountItems
                        Counter += 1
                        If Counter > 1 Then
                            Sw.WriteLine("        SQL.Append(" & Chr(34) & "     AND " & Chr(34) & ")")
                        End If

                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     ( " & Chr(34) & ")")
                        Dim Index As Integer = 0
                        Dim PreRel As String = "AND"
                        For Each KeyItem As CKeyItem In KeyList
                            Index += 1

                            If Index > 1 Then
                                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & PreRel & " " & Chr(34) & ")")
                            End If

                            If KeyItem.Method <> "" Then

                                If KeyItem.InternalMethod = "" Then
                                    If KeyItem.Method = "TODATETIME" Then
                                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     CONVERT(varchar(14), [" & KeyItem.Field.Id & "], 100) " & KeyItem.Oper & " @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                    ElseIf KeyItem.Method = "TODATE" Then
                                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     CONVERT(varchar(8), [" & KeyItem.Field.Id & "], 112) " & KeyItem.Oper & " @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                    Else
                                        Return False
                                    End If
                                Else
                                    Sw.WriteLine("        SQL.Append(" & Chr(34) & "     " & KeyItem.Method & " " & KeyItem.Oper & " @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                End If
                            Else
                                Sw.WriteLine("        SQL.Append(" & Chr(34) & "     [" & KeyItem.Field.Id & "] " & KeyItem.Oper & " " & Chr(34) & ")")
                                If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                    Sw.WriteLine("        SQL.Append(""     ("")")
                                    Sw.WriteLine("        Dim Idx1 As Integer")
                                    Sw.WriteLine("        For Idx1 = 1 To Obj." & KeyItem.Field.Id & "_Array.Count - 1")
                                    Sw.WriteLine("            SQL.Append(""       @" & KeyItem.Field.Id & "_"" + Idx1.ToString + "", "")")
                                    Sw.WriteLine("        Next")
                                    Sw.WriteLine("        SQL.Append(""       @" & KeyItem.Field.Id & "_"" + Idx1.ToString)")
                                    Sw.WriteLine("        SQL.Append(""     ) "")")
                                Else
                                    Sw.WriteLine("        SQL.Append(""       @" & KeyItem.Field.Id & " " & Chr(34) & ")")
                                End If
                            End If
                            PreRel = KeyItem.Relation
                        Next

                        Sw.WriteLine("        SQL.Append(" & Chr(34) & "     ) " & Chr(34) & ")")
                    Next

                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim Command As SqlClient.SqlCommand = Adapter.Command()")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Command.CommandType = CommandType.Text")
                    Sw.WriteLine("        Command.CommandText = SQL.ToString")

                    For Each KeyList As List(Of CKeyItem) In Count.CountItems
                        For Each KeyItem As CKeyItem In KeyList
                            If KeyItem.Method <> "" Then
                                If KeyItem.InternalMethod = "" Then
                                    If KeyItem.Method = "TODATETIME" Then
                                        Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & ".ToString(" & Chr(34) & "YYYYmmDDHHMMSS" & Chr(34) & "))")
                                    ElseIf KeyItem.Method = "TODATE" Then
                                        Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & ".ToString(" & Chr(34) & "YYYYmmDD" & Chr(34) & "))")
                                    Else
                                        Return False
                                    End If
                                Else
                                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & "." & KeyItem.InternalMethod & ")")
                                End If
                            Else
                                If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                    Sw.WriteLine("        For Idx2 As Integer = 0 To Obj.TestValue_Array.Count - 1")
                                    Sw.WriteLine("            Command.Parameters.AddWithValue(""" & KeyItem.Field.Id & "_"" + (Idx2 + 1).ToString, StructObj." & KeyItem.Field.Id & "_Array(Idx2))")
                                    Sw.WriteLine("        Next")
                                Else
                                    Sw.WriteLine("        Command.Parameters.AddWithValue(" & Chr(34) & KeyItem.Field.Id & Chr(34) & ", StructObj." & KeyItem.Field.Id & ")")
                                End If
                            End If
                        Next
                    Next

                    Sw.WriteLine("")
                    Sw.WriteLine("        Try")
                    Sw.WriteLine("            Return Command.ExecuteScalar()")
                    Sw.WriteLine("        Catch ex As Exception")
                    Sw.WriteLine("            Throw New CBusinessException(CErrorCode.DATABASE_QUERY_FAIL, ex)")
                    Sw.WriteLine("        End Try")
                    Sw.WriteLine("")
                    Sw.WriteLine("    End Function")
                Next
                Sw.WriteLine()
                Sw.WriteLine("    Public ReadOnly Property Name As String Implements IComponent.Name")
                Sw.WriteLine("        Get")
                Sw.WriteLine("            Return ""SQLServer_" + Table.Id + """")
                Sw.WriteLine("        End Get")
                Sw.WriteLine("    End Property")
                Sw.WriteLine()
                Sw.WriteLine("End Class")
                Sw.WriteLine("")

            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function GenerateDatabaseClass() As Boolean
        For Each Table As CTable In TableList
            If GenerateDatabaseClass(Table) = False Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Function GenerateDatabaseClass(ByVal Table As CTable) As Boolean

        Console.WriteLine("Genreate Database Class {0}", Table.Id)
        Dim DirInfo As IO.DirectoryInfo
        Try
            DirInfo = IO.Directory.GetParent(My.Application.Info.DirectoryPath)
            If Not IO.Directory.Exists(DirInfo.FullName & IO.Path.DirectorySeparatorChar & "Database") Then
                IO.Directory.CreateDirectory(DirInfo.FullName & IO.Path.DirectorySeparatorChar & "Database")
            End If

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try
        Dim File As String = DirInfo.FullName & IO.Path.DirectorySeparatorChar & "Database" & IO.Path.DirectorySeparatorChar & "C" & Table.Id & ".vb"
        Dim SavedContents As String = ""
        If IO.File.Exists(File) Then
            Try
                Using Sr As New IO.StreamReader(File)
                    Dim Content As String = Sr.ReadToEnd
                    Dim CurrentIdx As Integer = Content.IndexOf("#Region " & Chr(34) & "User Defined Code" & Chr(34))
                    If CurrentIdx >= 0 Then
                        Dim EndIndex As Integer = Content.IndexOf("#End Region", CurrentIdx)
                        If EndIndex > 0 Then
                            SavedContents = Content.Substring(CurrentIdx, EndIndex - CurrentIdx + 11) & vbCrLf
                        Else
                            Console.WriteLine("Warning : Invalid Database Class Library, Skip User Defined Codes")

                        End If
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Return False
            End Try
        End If

        Try
            Using Sw As New IO.StreamWriter(DirInfo.FullName & IO.Path.DirectorySeparatorChar & "Database" & IO.Path.DirectorySeparatorChar & "C" & Table.Id & ".vb")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("' File          : C" & Table.Id & ".vb")
                Sw.WriteLine("' Class         : C" & Table.Id & "")
                Sw.WriteLine("' Description   : Table : " & Table.Id & ", This Class is Generated By DBCG Tools Automatically")
                Sw.WriteLine("' Author        : Su Jia")
                Sw.WriteLine("' Creation Ver  : " + My.Application.Info.Version.ToString)
                Sw.WriteLine("'")
                Sw.WriteLine("' Revision History")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("' Version       Date                 User")
                Sw.WriteLine("' $Log: C" & Table.Id & ".vb,v $")
                Sw.WriteLine("' Revision 1.0  2012/08/24           Su Jia")
                Sw.WriteLine("' Initial Version")
                Sw.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''")
                Sw.WriteLine("Imports ServerPlatform.Library.Utility")
                Sw.WriteLine("Imports ServerPlatform.Library.Workflow")
                Sw.WriteLine("")
                For Each Item As String In Table.Attr
                    Sw.WriteLine(Item)
                Next
                Sw.WriteLine("Public Class C" & Table.Id & "")
                Sw.WriteLine("    Implements ICloneable")
                Sw.WriteLine("    Private Curser As New List(Of Object)")
                Sw.WriteLine("")
                For Each Field As CField In Table.Fields
                    Sw.WriteLine("    Public Property " & Field.Id.Replace("_", "") & " As " & Field.GetInternalDataType())
                Next
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub New()")
                For Each Field As CField In Table.Fields
                    Sw.WriteLine("        " & Field.Id.Replace("_", "") & " = " & Field.Value)
                Next

                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Private Sub ToSqlPK(ByRef DBStruct As C" & Table.Id & "Struct)")
                For Each Field As CField In Table.GetPrimaryKey
                    Sw.WriteLine("        DBStruct." & Field.Id & " = " & Field.Id.Replace("_", ""))
                Next

                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Private Sub ToSql(ByRef DBStruct As C" & Table.Id & "Struct)")
                Sw.WriteLine("        ToSqlPK(DBStruct)")
                For Each Field As CField In Table.GetFieldsWithoutPK
                    Sw.WriteLine("        DBStruct." & Field.Id & " = " & Field.Id.Replace("_", ""))
                Next
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Private Sub FromSql(ByVal DBStruct As C" & Table.Id & "Struct)")
                For Each Field As CField In Table.Fields
                    Sw.WriteLine("        " & Field.Id.Replace("_", "") & " = DBStruct." & Field.Id)
                Next
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Insert(ByRef DatabaseFactory As CDatabaseFactory)")
                Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                Sw.WriteLine("        ToSql(StructObj)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                Sw.WriteLine("        DBAccessClass.Insert(DatabaseFactory.CreateInstance, StructObj)")
                If Table.Fields(0).DataType = "IDENTITY" Then
                    Sw.WriteLine("        " + Table.Fields(0).Id + " = StructObj." + Table.Fields(0).Id)
                End If
                Sw.WriteLine("    End Sub")

                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Update(ByRef DatabaseFactory As CDatabaseFactory)")
                Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                Sw.WriteLine("        ToSql(StructObj)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                Sw.WriteLine("        DBAccessClass.Update(DatabaseFactory.CreateInstance, StructObj)")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Remove(ByRef DatabaseFactory As CDatabaseFactory)")
                Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                Sw.WriteLine("        ToSql(StructObj)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                Sw.WriteLine("        DBAccessClass.Remove(DatabaseFactory.CreateInstance, StructObj)")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Sub Search(ByRef DatabaseFactory As CDatabaseFactory, _")

                For Each Field As CField In Table.GetPrimaryKey
                    Sw.WriteLine("                    ByVal " & Field.Id & " As " & Field.GetInternalDataType & ", _")
                Next
                Sw.WriteLine("                    ByVal Lock As Boolean?)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                For Each Field As CField In Table.GetPrimaryKey
                    Sw.WriteLine("        Me." & Field.Id.Replace("_", "") & " = " & Field.Id)
                Next

                Sw.WriteLine("        ToSqlPK(StructObj)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                Sw.WriteLine("        DBAccessClass.Search(DatabaseFactory.CreateInstance, StructObj, Lock)")
                Sw.WriteLine("        FromSql(StructObj)")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Shared Sub SearchAll(ByRef DatabaseFactory As CDatabaseFactory, ByRef ObjList As List(Of C" & Table.Id & "))")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(""" + Table.Id + """)")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObjList As New List(Of Object)")
                Sw.WriteLine("        DBAccessClass.SearchAll(DatabaseFactory.CreateInstance, StructObjList)")
                Sw.WriteLine("")
                Sw.WriteLine("        For Each StructObj As C" & Table.Id & "Struct In StructObjList")
                Sw.WriteLine("            Dim Obj As New C" & Table.Id)
                Sw.WriteLine("            Obj.FromSql(StructObj)")
                Sw.WriteLine("            ObjList.Add(Obj)")
                Sw.WriteLine("        Next")
                Sw.WriteLine("    End Sub")
                Sw.WriteLine("")
                For Each Query As CQuery In Table.Query
                    Sw.WriteLine("")
                    Sw.WriteLine("    Public Sub Search" & Query.Id & "(ByRef DatabaseFactory As CDatabaseFactory, _")
                    Dim Counter As Integer = 0
                    For Each KeyList As List(Of CKeyItem) In Query.Queries

                        For Each KeyItem As CKeyItem In KeyList
                            Counter += 1
                            Dim FieldDesc As String = KeyItem.Field.GetInternalDataType
                            If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                FieldDesc = "List(Of " + FieldDesc + ")"
                            End If

                            Dim RepeartCount As String = ""
                            If KeyItem.Repeat >= 2 Then RepeartCount = Counter.ToString
                            Sw.WriteLine("                    ByVal " & KeyItem.Field.Id & RepeartCount & " As " & FieldDesc & ", _")
                        Next
                    Next
                    Sw.WriteLine("                    ByVal Index As Integer, ByVal Lock As Boolean?, Optional ByVal MaxRecord As Integer = 0)")
                    Sw.WriteLine("        If Curser.Count > 0 Then")
                    Sw.WriteLine("            If Index <= Curser.Count - 1 Then")
                    Sw.WriteLine("                FromSql(Curser.Item(Index))")
                    Sw.WriteLine("                Exit Sub")
                    Sw.WriteLine("            Else")
                    Sw.WriteLine("                Throw New CError.CBusinessException(CError.CErrorCode.RECORD_NOT_FOUND, ""Index Out Of Range"")")
                    Sw.WriteLine("            End If")
                    Sw.WriteLine("        End If")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                    For Each KeyList As List(Of CKeyItem) In Query.Queries
                        Counter = 0
                        For Each KeyItem As CKeyItem In KeyList
                            Counter += 1
                            ' Sw.WriteLine("        Me." & KeyItem.Field.Id.Replace("_", "") & " = " & KeyItem.Field.Id)
                            If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                Sw.WriteLine("        StructObj." & KeyItem.Field.Id & "_Array = " & KeyItem.Field.Id)
                            Else
                                If KeyItem.Repeat >= 2 Then
                                    Sw.WriteLine("        StructObj." & KeyItem.Field.Id & "_Array.Add(" & KeyItem.Field.Id & Counter.ToString & ")")
                                Else
                                    Sw.WriteLine("        StructObj." & KeyItem.Field.Id & " = " & KeyItem.Field.Id)
                                End If

                            End If
                        Next
                    Next
                    Sw.WriteLine("        StructObj.MaxRecord = MaxRecord")

                    'Sw.WriteLine("")
                    'Sw.WriteLine("        ToSql(StructObj)")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                    Sw.WriteLine("        DBAccessClass.Search" & Query.Id & "(DatabaseFactory.CreateInstance, StructObj, Curser, Lock)")
                    Sw.WriteLine("")
                    Sw.WriteLine("        FromSql(Curser.Item(Index))")
                    Sw.WriteLine("")
                    Sw.WriteLine("    End Sub")
                Next

                Sw.WriteLine("")
                Sw.WriteLine("    Public Function Count(ByRef DatabaseFactory As CDatabaseFactory) As Integer")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                Sw.WriteLine("")
                Sw.WriteLine("        Dim DBAccessClass As IDatabaseAccess = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                Sw.WriteLine("        Return DBAccessClass.Count(DatabaseFactory.CreateInstance)")
                Sw.WriteLine("    End Function")

                For Each Count As CCount In Table.Count

                    Sw.WriteLine("")
                    Sw.WriteLine("    Public Function Count" & Count.Id & "(ByRef DatabaseFactory As CDatabaseFactory, _")
                    For Each KeyList As List(Of CKeyItem) In Count.CountItems
                        Dim Idx As Integer = 0
                        For Each KeyItem As CKeyItem In KeyList

                            Dim FieldDesc As String = KeyItem.Field.GetInternalDataType
                            If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                FieldDesc = "List(Of " + FieldDesc + ")"
                            End If

                            Idx += 1
                            If KeyList.Count = Idx Then
                                Sw.WriteLine("                    ByVal " & KeyItem.Field.Id & " As " & FieldDesc & ") As Integer")
                            Else
                                Sw.WriteLine("                    ByVal " & KeyItem.Field.Id & " As " & FieldDesc & ", _")
                            End If

                        Next
                    Next
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim StructObj As New C" & Table.Id & "Struct")
                    For Each KeyList As List(Of CKeyItem) In Count.CountItems
                        For Each KeyItem As CKeyItem In KeyList
                            ' Sw.WriteLine("        Me." & KeyItem.Field.Id.Replace("_", "") & " = " & KeyItem.Field.Id)
                            If KeyItem.Oper = "IN" Or KeyItem.Oper = "NOT IN" Then
                                Sw.WriteLine("        StructObj." & KeyItem.Field.Id & "_Array = " & KeyItem.Field.Id)
                            Else
                                Sw.WriteLine("        StructObj." & KeyItem.Field.Id & " = " & KeyItem.Field.Id)
                            End If

                        Next
                    Next

                    ' Sw.WriteLine("")
                    'Sw.WriteLine("        ToSql(StructObj)")
                    Sw.WriteLine("")
                    Sw.WriteLine("        Dim DBAccessClass As Object = DatabaseFactory.GetDatabaseAccessLibrary(ClassName)")
                    Sw.WriteLine("        Return DBAccessClass.Count" & Count.Id & "(DatabaseFactory.CreateInstance, StructObj)")
                    Sw.WriteLine("")
                    Sw.WriteLine("    End Function")
                Next

                Sw.WriteLine("")
                Sw.WriteLine("    Public ReadOnly Property ClassName As String")
                Sw.WriteLine("        Get")
                Sw.WriteLine("            Return " & Chr(34) & "" & Table.Id & "" & Chr(34) & "")
                Sw.WriteLine("        End Get")
                Sw.WriteLine("    End Property")
                Sw.WriteLine("")
                Sw.WriteLine("    Public Overridable Function Clone() As Object Implements ICloneable.Clone")
                Sw.WriteLine("        Dim " & Table.Id & " As New C" & Table.Id)
                For Each Field As CField In Table.Fields
                    Sw.WriteLine("        " + Table.Id + "." & Field.Id.Replace("_", "") & " = " & Field.Id.Replace("_", ""))
                Next
                Sw.WriteLine("        Return " & Table.Id)
                Sw.WriteLine("    End Function")
                Sw.WriteLine("")
                Sw.WriteLine(SavedContents)
                Sw.WriteLine("")
                Sw.WriteLine("End Class")
                Sw.WriteLine("")
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function GenerateSQL()
        For Each Table As CTable In TableList
            If CreateSQL(Table) = False Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function CreateSQL(ByRef Table As CTable) As Boolean
        Dim Output As String = Table.Id & ".sql"
        Dim Context As String = ""
        Try
            Dim SrFile As IO.StreamWriter
            '读取文件至数据流
            SrFile = New System.IO.StreamWriter(Output, False)

            SrFile.WriteLine("--------------------------------------------------------------------------------")
            SrFile.WriteLine("-- This File was Automaticly Created By DBAC Tools ")
            SrFile.WriteLine("-- Create Ver : " + My.Application.Info.Version.ToString)
            SrFile.WriteLine("--------------------------------------------------------------------------------")
            SrFile.WriteLine()
            SrFile.WriteLine("CREATE TABLE " & Table.Id.ToUpper)
            SrFile.WriteLine("(")

            Dim PrimaryKey As String = ""
            For IntLoop As Int32 = 0 To Table.Fields.Count - 1
                Dim Buffer As String = ""
                Dim Item As CField
                Item = Table.Fields(IntLoop)
                Buffer = ""
                Buffer &= "    " & String.Format("{0,-20:G}", Item.Id.ToUpper)
                If Item.DataType = "VARCHAR" Then
                    Buffer &= "    " & String.Format("{0,-10:G}", "VARCHAR")
                ElseIf Item.DataType = "INT" And Item.Length = 2 Then
                    Buffer &= "    " & String.Format("{0,-10:G}", "INT")
                ElseIf Item.DataType = "INT" And Item.Length = 4 Then
                    Buffer &= "    " & String.Format("{0,-10:G}", "INT")
                ElseIf Item.DataType = "INT" And Item.Length = 8 Then
                    Buffer &= "    " & String.Format("{0,-10:G}", "LONG")
                Else
                    Buffer &= "    " & String.Format("{0,-10:G}", Item.DataType.ToUpper)
                End If
                If Not (Item.DataType = "CHAR" Or Item.DataType = "VARCHAR") Then
                    Buffer &= "     "
                Else
                    Buffer &= String.Format("{0,-5:G}", "(" & Item.Length & ")")
                End If

                If Item.NotNull = True Then
                    Buffer &= " NOT NULL,"
                Else
                    Buffer &= " ,        "
                End If
                Dim IsFirst As Boolean = True
                If Item.Key = True Then
                    If IsFirst = True Then
                        PrimaryKey = Item.Id
                    Else
                        PrimaryKey = PrimaryKey & ", " & Item.Id
                    End If
                End If
                SrFile.WriteLine(Buffer)
            Next

            SrFile.WriteLine("    CONSTRAINT PKC_" & Table.Id & " PRIMARY KEY (" & PrimaryKey.ToUpper & ")")
            SrFile.WriteLine(");")
            SrFile.WriteLine()

            Dim IsFirst1 As Boolean = True
            Dim Buffer1 As String = ""
            For IntLoop As Int32 = 0 To Table.GetPrimaryKey.Count - 1
                If IsFirst1 = True Then
                    Buffer1 = Table.GetPrimaryKey(IntLoop).Id
                Else
                    Buffer1 = Buffer1 & ", " & Table.GetPrimaryKey(IntLoop).Id
                End If
            Next
            SrFile.WriteLine("CREATE INDEX " & Table.Id.ToUpper & "_PK ON " & Table.Id & _
                    "(" & Buffer1 & ")")
            SrFile.WriteLine()

            SrFile.Close()

            Return True
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return False
        End Try

        Return True
    End Function

    Private Enum Section As Integer
        TABLE = 0
        QUERY = 1
        COUNT = 2
        UNKNOWN = 3
    End Enum
End Module
