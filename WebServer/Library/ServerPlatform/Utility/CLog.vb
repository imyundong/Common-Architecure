Imports ServerPlatform.Library.Data

Namespace Utility
    Public Class CLog
        ' Log Information
        Private Shared LogInfo As CLogInfo
        ' Sync Object
        Private Shared SyncObject As New Object

        Private Shared SyncronizedQ As Queue

        Private Shared IsIntialized As Boolean = False

        Public Shared Sub Init(ByVal Info As CLogInfo)
            If IsIntialized = True Then
                Exit Sub
            End If

            SyncLock SyncObject
                If IsIntialized = True Then
                    Exit Sub
                End If

                ' Set Value
                LogInfo = Info
                LogInfo.Init()

                ' Use Syncronized Queue 
                Dim LogQ As New Queue
                SyncronizedQ = Queue.Synchronized(LogQ)
                ' Start new Thread to Write Log
                System.Threading.ThreadPool.QueueUserWorkItem(AddressOf Write)

                IsIntialized = True
            End SyncLock
        End Sub

        Private Shared Sub Write()
            Dim Path As String = CUtility.ServerPath(LogInfo.LogPath)
            If Not IO.Directory.Exists(Path) Then
                IO.Directory.CreateDirectory(Path)
            End If

            Dim File As New Text.StringBuilder()
            ' Seprate the log by domain
            ' If (Not AppDomain.CurrentDomain.IsDefaultAppDomain) And CEnviroment.IsWebApplication = False Then
            File.Append(AppDomain.CurrentDomain.FriendlyName + ".")
            ' End If

            File.Append(LogInfo.Prefix + ".Log.")
            File.Replace("/", "")

            Dim FileList() As String = IO.Directory.GetFiles(Path, File.ToString + "*")
            Dim CurrentIndex As Integer = 0
            Dim CurrentLogSize As Long = 0

            Dim LastAccessDate As Date
            ' Get Last File Sequence Number
            For Each TmpFile As String In FileList
                Dim Info As New IO.FileInfo(TmpFile)
                If Info.LastWriteTime > LastAccessDate Then
                    LastAccessDate = Info.LastWriteTime
                    Dim Tmp() As String = TmpFile.Split(".")
                    Integer.TryParse(Tmp(Tmp.Count - 1), CurrentIndex)
                End If
            Next

            CurrentIndex += 1
            Dim Sw As IO.StreamWriter = OpenFile(CurrentIndex, Path + IO.Path.DirectorySeparatorChar + File.ToString)

            While (True)

                Dim LogContent As CLogContent
                If SyncronizedQ.Count = 0 Then
                    LogContent = Nothing
                Else
                    LogContent = SyncronizedQ.Dequeue()
                End If

                While Not LogContent Is Nothing
                    Dim Content As String = FormatContent(LogContent.Format, LogContent.Values)
                    CurrentLogSize += Content.Length

                    If CurrentLogSize > LogInfo.LogSize Then
                        ' Reset File Size
                        CurrentLogSize = 0
                        ' Increase File Size
                        CurrentIndex += 1
                        ' Close File
                        Sw.Close()
                        ' Reopen the File
                        Sw = OpenFile(CurrentIndex, Path + IO.Path.DirectorySeparatorChar + File.ToString)
                    End If
                    ' Write Log
                    Sw.WriteLine(Content)

                    If SyncronizedQ.Count = 0 Then
                        LogContent = Nothing
                    Else
                        LogContent = SyncronizedQ.Dequeue()
                    End If
                End While
                Sw.Flush()
                Threading.Thread.Sleep(50)
            End While
        End Sub

        Private Shared Function FormatContent(ByVal Format As String, ParamArray Values As Object()) As String
            If Values Is Nothing OrElse Values.Length = 0 Then
                Return Format
            Else
                Return String.Format(Format, Values)
            End If
        End Function

        Private Shared Function OpenFile(ByRef CurrentIndex As Integer, ByVal FilePath As String) As IO.StreamWriter
            If CurrentIndex > LogInfo.LogCount Then
                CurrentIndex = 1
            End If

            Dim Filename As String = FilePath.ToString + Format(CurrentIndex, New String("0", LogInfo.LogCount.ToString.Length))
            Return New IO.StreamWriter(Filename)
        End Function

        Public Shared Sub Sys(ByVal Format As String, ByVal ParamArray Values As Object())
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("SYSTEM") Then
                SyncronizedQ.Enqueue(New CLogContent("S:" + Format, Values))
            End If

        End Sub

        Public Shared Sub Debug(ByVal Key As String, ByVal Value As String)
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("DEBUG") Then
                SyncronizedQ.Enqueue(New CLogContent(String.Format("D:{0, -15:G} : {1}", Key, Value), Nothing))
            End If
        End Sub

        Public Shared Sub Debug(ByVal Info As String)
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("DEBUG") Then
                SyncronizedQ.Enqueue(New CLogContent("D:" + Info, Nothing))
            End If
        End Sub

        Public Shared Sub Dump(ByVal Message As CMessage)
            SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} {1}", "Txn Code", Message.TxnCode))
            SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} {1}", "Txn Date", Message.TxnDate))

            SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} {1}", "System Date", Message.SystemDate))
            SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} {1}", "Error", Message.ErrCode))
            For i As Integer = 0 To Message.Keys.Count - 1
                Dim Value As Object
                If Message.Values(i) Is Nothing Then
                    Value = "null"
                    SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} null", Message.Keys(i)))
                Else
                    Value = Message.Values(i)
                    If Value.GetType Is GetType(String) Then
                        Dim Values() As String = Value.ToString.Split(vbCrLf)
                        SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} {1}", "Txn Code", Message.Keys(i), Values(0)))
                        For j As Integer = 1 To Values.Count - 1

                            SyncronizedQ.Enqueue(New CLogContent("U:                " + Values(j).Replace(vbLf, "").Replace(vbCr, "")))
                        Next
                    Else
                        SyncronizedQ.Enqueue(New CLogContent("U:{0, -15:G} null", Message.Keys(i)))
                    End If
                End If
            Next
        End Sub


        Private Shared Sub BinaryDump(ByRef Buffer As Byte())
            Dim Temp As New Text.StringBuilder("U:")
            If LogInfo.LogLevelCollection.Contains("DUMP") Then
                SyncronizedQ.Enqueue(New CLogContent("U:00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F"))
                SyncronizedQ.Enqueue(New CLogContent("U:-- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --"))

                For Idx As Integer = 0 To Buffer.Length - 1
                    Temp.Append(Buffer(Idx).ToString("X2") + " ")
                    If (Idx + 1) Mod 16 = 0 Then
                        SyncronizedQ.Enqueue(New CLogContent(Temp.ToString, Nothing))
                        Temp.Clear()
                        Temp.Append("U:")
                    End If
                Next

                If Buffer.Count Mod 16 <> 0 Then
                    SyncronizedQ.Enqueue(New CLogContent(Temp.ToString, Nothing))
                End If
                SyncronizedQ.Enqueue(New CLogContent("U:-- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --"))

                CLog.Debug(Text.Encoding.UTF8.GetString(Buffer))
            End If
        End Sub

        Public Shared Sub Dump(ByVal Buffer() As Byte)
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            BinaryDump(Buffer)
        End Sub

        Public Shared Sub Dump(ByRef Buffer As IO.Stream)
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            Buffer.Position = 0
            Dim Buf(Buffer.Length - 1) As Byte
            Buffer.Read(Buf, 0, Buffer.Length)
            Buffer.Position = 0

            BinaryDump(Buf)
        End Sub

        Public Shared Sub Info(ByVal Format As String, ByVal ParamArray Values As Object())
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("INFO") Then
                SyncronizedQ.Enqueue(New CLogContent("I:" + Format, Values))
            End If

        End Sub

        Public Shared Sub Warning(ByVal Format As String, ByVal ParamArray Values As Object())
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("WARNING") Then
                SyncronizedQ.Enqueue(New CLogContent("W:" + Format, Values))
            End If

        End Sub

        Public Shared Sub Err(ByVal Format As String, ByVal Exception As Exception)
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("ERROR") Then
                SyncronizedQ.Enqueue(New CLogContent("E:" + Format + " {{{" + Exception.Message + "}}}"))
            End If

        End Sub


        Public Shared Sub Err(ByVal Format As String, ByVal ParamArray Values As Object())
            ' Check Initialize Status
            If IsIntialized = False Then Exit Sub

            If LogInfo.LogLevelCollection.Contains("ERROR") Then
                SyncronizedQ.Enqueue(New CLogContent("E:" + Format, Values))
            End If

        End Sub

        Private Class CLogContent
            Property Format As String
            Property Values As Object()

            Public Sub New(Format As String, ParamArray Values As Object())
                Me.Format = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString("00000") + " " + Now.ToString("[yyyyMMdd HH:mm:ss:fff] ") + Format
                Me.Values = Values
            End Sub
        End Class

    End Class
End Namespace
