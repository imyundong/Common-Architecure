Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility

Public Class BLTellerNotify
    Inherits CTaskBase
    Public Overrides ReadOnly Property Name As String
        Get
            Return "BLTellerNotify"
        End Get
    End Property

    Public Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
        CLog.Info("Teller Notify Batch Job")
        Dim AppConfig As CTaskScheduleConfig = CTaskScheduleConfig.GetInstance()

        Dim Files As String() = IO.Directory.GetFiles(AppConfig.Source, "Notify_*.dat")
        For Each File As String In Files
            Try
                Using Sr As New IO.StreamReader(File)
                    Try
                        ' Create Output
                    Dim FileInfo As New IO.FileInfo(File)
                    Dim FileName As String = FileInfo.DirectoryName + IO.Path.DirectorySeparatorChar + "DONE_" + FileInfo.Name
                    Using Sw As New IO.StreamWriter(FileName)
                        Dim Content As String = Sr.ReadLine
                            While Not String.IsNullOrEmpty(Content)
                                Try
                                    Dim Fields As String() = Content.Split("|"c)
                                    Dim Result As Boolean = False
                                    If Fields.Count <> 5 Then
                                        WriteError(Sw, CErrCode.INVALID_FIELD_COUNT, Content)
                                        Content = Sr.ReadLine
                                        Continue While
                                    End If

                                    If Fields(0).ToUpper <> "TELLER" Then
                                        WriteError(Sw, CErrCode.INVALID_MESSAGE_TYPE, Content)
                                        Content = Sr.ReadLine
                                        Continue While
                                    End If

                                    Dim TellerNo As Integer = 0
                                    If Integer.TryParse(Fields(1), TellerNo) = False Then
                                        WriteError(Sw, CErrCode.INVALID_TELLER_ID, Content)
                                        Content = Sr.ReadLine
                                        Continue While
                                    End If

                                    Dim SendTeller As Integer = 0
                                    If Integer.TryParse(Fields(2), SendTeller) = False Then
                                        WriteError(Sw, CErrCode.INVALID_SENDER_TELLER, Content)
                                        Content = Sr.ReadLine
                                        Continue While
                                    End If

                                    Dim UserToDoQueue As New CUserToDoQueue
                                    Try
                                        UserToDoQueue.SearchByID(DatabaseFactory, TellerNo, 0, False)
                                    Catch ex As Exception
                                        CLog.Err("Invalid Teller ID or Can not Find TODO Queue : {0}", ex.Message)
                                        WriteError(Sw, CErrCode.INVALID_TELLER_ID, Content)
                                        Content = Sr.ReadLine
                                        Continue While
                                    End Try

                                    Try
                                        Dim QueueItem As New CQueueItem
                                        Dim CurrentDate As Date = Now()
                                        QueueItem.Capability = 0
                                        QueueItem.DateAdded = CInt(CurrentDate.ToString("yyyyMMdd"))
                                        QueueItem.DateExpired = CInt(CurrentDate.AddDays(3).ToString("yyyyMMdd"))
                                        QueueItem.DateProcessed = CInt(CurrentDate.ToString("yyyyMMdd"))
                                        QueueItem.Description = Fields(3)
                                        QueueItem.OriginalItemID = 0
                                        QueueItem.Priority = 2
                                        QueueItem.ProcessorTellerID = TellerNo
                                        QueueItem.SenderTellerID = SendTeller
                                        QueueItem.QueueID = UserToDoQueue.QueueID
                                        QueueItem.Status = 2
                                        QueueItem.TimeAdded = CInt(CurrentDate.ToString("HHmmss"))
                                        QueueItem.XMLDocument = Fields(4)

                                        QueueItem.Insert(DatabaseFactory)
                                    Catch ex As Exception
                                        CLog.Err(ex.Message)
                                        WriteError(Sw, CErrCode.INVALID_MESSAGECONTENT, Content)
                                        Content = Sr.ReadLine
                                        Continue While
                                    End Try

                                    WriteError(Sw, CErrCode.SUCCESSFUL, Content)
                                    Content = Sr.ReadLine
                                    'TELLER|65679|65674|新增計算機功能|請使用快速工具列上的<img src='../Images/NEW/calculator0_new.png' >

                                Catch ex As Exception
                                    CLog.Err(ex.ToString)
                                    WriteError(Sw, CErrCode.INVALID_MESSAGECONTENT, Content)
                                    Content = Sr.ReadLine
                                End Try
                            End While
                        End Using

                    Catch ex As Exception
                        CLog.Err(ex.ToString)
                    End Try
                End Using
            Catch ex As Exception
                CLog.Err(ex.ToString)
            End Try

            IO.File.Delete(File)
        Next

    End Sub

    Sub WriteError(ByRef Sw As IO.StreamWriter, ByVal ErrCode As CErrCode, ByVal Content As String)
        If ErrCode = CErrCode.SUCCESSFUL Then
            Sw.Write("SUCCESS|")
        Else
            Sw.Write("ERROR|")
        End If
        Sw.Write(CInt(ErrCode).ToString("0000"))
        Sw.WriteLine(Content)
    End Sub
End Class

Public Enum CErrCode As Integer
    SUCCESSFUL = 0
    INVALID_FIELD_COUNT = 1
    INVALID_MESSAGE_TYPE = 2
    INVALID_TELLER_ID = 3
    INVALID_SENDER_TELLER = 4
    INVALID_DESCRIPTION = 5
    INVALID_MESSAGECONTENT = 6
End Enum


