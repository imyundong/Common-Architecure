Imports ServerPlatform.AppServer
Imports ServerPlatform.Library.Workflow.CError
Imports System.Threading

Module Entry
    Dim AppServer As New CSystemCore
    Dim Status As String = "Ready"
    Dim DisplayEvent As New ManualResetEventSlim(True)

    <MTAThread>
    Sub Main()
        Console.WriteLine("Starting...")
        Dim ErrCode As CErrorCode = AppServer.Start()
        Console.Clear()
        If ErrCode <> CErrorCode.SUCCESSFUL Then Status = "Fatal Error"

        DisplayMenu()
        While (True)
            Console.Write("Please Select : ")
            Dim Value As ConsoleKeyInfo = Console.ReadKey

            If Value.KeyChar = "1" Then
                If DisplayEvent.IsSet = False Then
                    Continue While
                End If
                DisplayEvent.Reset()
                ThreadPool.QueueUserWorkItem(AddressOf DisplayStatusTask)
            Else
                DisplayMenu()
                DisplayEvent.Set()
                If Value.KeyChar = "3" Then
                    Console.WriteLine()
                    Console.WriteLine("Application List")
                    Console.WriteLine("--------------------------------------------------")
                    For Each Item In AppServer.ApplicationList
                        If Item.Value.AppStatus.Status = ServerPlatform.Library.Data.CAppStatus.CRuntimeStatus.Started Then
                            Console.WriteLine("Id : " + Item.Key)

                        End If
                    Next
                    Console.WriteLine("--------------------------------------------------")
                    Console.Write("Stop (Application Id) : ")
                    Dim AppId As String = Console.ReadLine
                    If String.IsNullOrEmpty(AppId) Then
                        DisplayMenu()
                        Continue While
                    End If
                    Console.WriteLine("Stopping...")
                    AppServer.StopApplication(AppId)

                    DisplayStatus()
                ElseIf Value.KeyChar = "2" Then
                    Console.WriteLine()
                    Console.WriteLine("Application List")
                    Console.WriteLine("--------------------------------------------------")
                    For Each Item In AppServer.ApplicationList
                        If Item.Value.AppStatus.Status = ServerPlatform.Library.Data.CAppStatus.CRuntimeStatus.Stopped Or Item.Value.AppStatus.Status = ServerPlatform.Library.Data.CAppStatus.CRuntimeStatus.Failed Then
                            Console.WriteLine("Id : " + Item.Key)
                        End If
                    Next
                    Console.WriteLine("--------------------------------------------------")
                    Console.Write("Start (Application Id) : ")
                    Dim AppId As String = Console.ReadLine
                    If String.IsNullOrEmpty(AppId) Then
                        DisplayMenu()
                        Continue While
                    End If
                    Console.WriteLine("Starting...")
                    AppServer.StartApplication(AppId)

                    DisplayStatus()
                End If

            End If
        End While

        Console.ReadKey()

    End Sub

    Sub DisplayMenu()
        Console.Clear()
        Console.WriteLine("Menu")
        Console.WriteLine("--------------------------------------------------")
        Console.WriteLine("1 : Display Application Status")
        Console.WriteLine("2 : Start Application")
        Console.WriteLine("3 : Stop Application")
        Console.WriteLine("--------------------------------------------------")
    End Sub

    Sub DisplayStatus()
        Console.Clear()
        Console.WriteLine("                             Application Platform Statics v0.1")
        Console.WriteLine()
        Console.WriteLine("Status : {0}                                                          Start Date : {1}", Status, Now)
        Console.WriteLine()
        Console.WriteLine("Seq App Id               Status          Running    Total Txn  Proc Time       Error Code")
        Console.WriteLine("--- -------------------- --------------- ---------- ---------- --------------- -------------------------")
        Dim Idx As Integer = 0
        Dim AvailableApp As Integer = 0
        For Each Item In AppServer.ApplicationList
            Idx += 1
            Console.WriteLine("{6, -3} {0, -20} {1, -15} {2, -10} {3, -10} {4, -15} {5, -25}", Item.Key, Item.Value.AppStatus.Status, Item.Value.AppStatus.TxnCount, Item.Value.AppStatus.TotalTxnCount, Item.Value.AppStatus.AverageProcTime.ToString("0.000000"), Item.Value.AppStatus.ErrCode, Idx.ToString("000"))
            If Item.Value.AppStatus.Status = ServerPlatform.Library.Data.CAppStatus.CRuntimeStatus.Started Then
                AvailableApp += 1
            End If
        Next
        Console.WriteLine("--- -------------------- --------------- ---------- ---------- --------------- -------------------------")
        Console.WriteLine("Available Application : {0}", AvailableApp)
        Thread.Sleep(200)
    End Sub

    Sub DisplayStatusTask()
        While True
            If DisplayEvent.IsSet Then
                Exit Sub
            End If
            DisplayStatus()

        End While
    End Sub
End Module

