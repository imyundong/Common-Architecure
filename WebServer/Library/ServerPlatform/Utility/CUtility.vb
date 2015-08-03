Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Data
Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices

Namespace Utility

    Namespace GenericType
        Public Class CUtility(Of T)

            Public Shared Sub WriteToSHM(ByVal Name As String, ByVal Target As T)
                Dim Fullname As String = "SharedMemory_" + Name
                CLog.Info("Write to Shared Memory : {0}", Fullname)
                Dim MappedFile As MemoryMappedFile = CUtility.AttachToSHM(Fullname)

                Dim Locker As New CReadWriteLocker(Name)
                Try
                    Locker.AcquireWriterLock()

                    Using Stream = MappedFile.CreateViewStream()
                        Dim XmlSerilizer As New Xml.Serialization.XmlSerializer(GetType(T))
                        'Dim Buffer As Byte() = Text.Encoding.ASCII.GetBytes(New String(" ", CUtility.SHM_SIZE))
                        'Stream.Write(Buffer, 0, 1)

                        For i As Integer = 0 To CUtility.SHM_SIZE - 1
                            Stream.WriteByte(32)
                        Next

                        Stream.Position = 0
                        XmlSerilizer.Serialize(Stream, Target)
                    End Using
                Catch ex As Exception
                    CLog.Warning("Write to Shared Memory({0}) Fail : {1}", Fullname, ex.Message)
                Finally
                    Locker.ReleaseWriterLock()
                End Try
            End Sub


            Public Shared Function ReadFromSHM(ByVal Name As String) As T
                Dim Fullname As String = "SharedMemory_" + Name
                CLog.Info("Retrieve From Shared Memory : {0}", Fullname)
                Dim MappedFile As MemoryMappedFile = CUtility.AttachToSHM(Fullname)
                Dim Locker As New CReadWriteLocker(Name)
                Try
                    Locker.AcquireReaderLock()
                    Using Stream = MappedFile.CreateViewStream()
                        ' Instead Of Structure, Class is using Xml Serilizer to read/write data
                        Dim XmlSerilizer As New Xml.Serialization.XmlSerializer(GetType(T))
                        Dim MappingData As T
                        MappingData = XmlSerilizer.Deserialize(Stream)

                        Return MappingData
                    End Using

                Catch ex As Exception
                    CLog.Warning("Retrieve From Shared Memory({0}) Fail : {1}", Fullname, ex.Message)
                Finally
                    Locker.ReleaseReaderLock()
                End Try
                Return Nothing
            End Function
        End Class

      
    End Namespace
    Public Class CUtility(Of T As Structure)

        Public Shared Sub WriteToSHM(ByVal Name As String, ByVal Target As Generic.List(Of T))
            Dim Fullname As String = "SharedMemory_" + Name
            CLog.Info("Write to Shared Memory : {0}", Fullname)
            Dim MappedFile As MemoryMappedFile = CUtility.AttachToSHM(Fullname)

            Dim Locker As New CReadWriteLocker(Name)
            Try
                Locker.AcquireWriterLock()

                Using Stream = MappedFile.CreateViewAccessor()
                    Dim Offset As Integer = 0
                    ' Write the record count
                    Stream.Write(0, Target.Count)
                    ' Write all object
                    Offset = Marshal.SizeOf(Target.Count)

                    For Each Item As Object In Target
                        Dim Size As Integer = Marshal.SizeOf(Item)
                        Stream.Write(Offset, Item)
                        Offset += Size
                    Next
                End Using
            Catch ex As Exception
                CLog.Warning("Retrieve From Shared Memory({0}) Fail : {1}", Fullname, ex.Message)
            Finally
                Locker.ReleaseWriterLock()
            End Try
        End Sub

        Public Shared Function ReadFromSHM(ByVal Name As String) As T()
            Dim Fullname As String = "SharedMemory_" + Name
            CLog.Info("Retrieve From Shared Memory : {0}", Fullname)
            Dim MappedFile As MemoryMappedFile = CUtility.AttachToSHM(Fullname)

            Dim Locker As New CReadWriteLocker(Fullname)
            Try
                Locker.AcquireReaderLock()

                Using Stream = MappedFile.CreateViewAccessor()
                    Dim RecordCount As Integer = Stream.ReadInt32(0)

                    Dim MappingData(RecordCount - 1) As T
                    Stream.ReadArray(Of T)(Marshal.SizeOf(0), MappingData, 0, RecordCount)

                    Return MappingData
                End Using

            Catch ex As Exception
                CLog.Warning("Retrieve From Shared Memory({0}) Fail : {1}", Fullname, ex.Message)
            Finally
                Locker.ReleaseReaderLock()
            End Try
            Return Nothing
        End Function
    End Class
    Public Class CUtility
        Private Shared SharedMemoryList As New Dictionary(Of String, MemoryMappedFile)
        Private Shared SHMSyncObject As New Object
        Public Const SHM_SIZE As Integer = 655360

        Public Shared Function AttachToSHM(ByVal Name As String)
            If SharedMemoryList.ContainsKey(Name) Then
                Return SharedMemoryList(Name)
            End If

            SyncLock SHMSyncObject
                If SharedMemoryList.ContainsKey(Name) Then
                    Return SharedMemoryList(Name)
                End If

                Dim MappedFile As MemoryMappedFile = MemoryMappedFile.CreateOrOpen(Name, CUtility.SHM_SIZE)
                SharedMemoryList.Add(Name, MappedFile)

                Return MappedFile
            End SyncLock
        End Function

        Public Shared Function ServerPath(ByVal Path As String) As String
            ' ASP.NET Application
            'If Not String.IsNullOrEmpty(CEnviroment.ServerPath) Then
            '    Return CEnviroment.ServerPath + IO.Path.DirectorySeparatorChar + Path
            'ElseIf System.Web.HttpContext.Current Is Nothing Then
            '    CEnviroment.ServerPath = Path
            '    Return Path
            'Else
            '    CEnviroment.ServerPath = System.Web.HttpContext.Current.Server.MapPath(Path)
            '    Return System.Web.HttpContext.Current.Server.MapPath(Path)
            'End If
            If IO.Path.IsPathRooted(Path) = False Then
                Return AppDomain.CurrentDomain.BaseDirectory + Path
            Else
                Return Path
            End If

        End Function

        Public Shared ComponentList As New Generic.Dictionary(Of String, Type)
        Private Shared LoadedAssemblyList As New List(Of String)
        Private Shared SyncObject As New Object

        ''' <summary>
        ''' Load Available Components From File
        ''' </summary>
        ''' <param name="Path">Dll Path</param>
        ''' <param name="AssemblyName">Filename</param>
        ''' <param name="Interfaces">Not Implemented</param>
        ''' <returns>Dictionary of Components</returns>
        ''' <remarks></remarks>
        Public Shared Function ComponentLoader(ByVal Path As String, ByVal AssemblyName As String, _
                                               ParamArray Interfaces As Type()) As Generic.Dictionary(Of String, Type)
            ' Return All Components Of Txn
            Dim CompList As New Generic.Dictionary(Of String, Type)
            If LoadedAssemblyList.Contains(AssemblyName) Then
                For Each Item In ComponentList
                    If Item.Key.StartsWith(AssemblyName & ".") Then
                        CompList.Add(Item.Key, Item.Value)
                    End If
                Next

                Return CompList
            End If

            SyncLock SyncObject
                ' Get Full Path
                Dim FullPath As String = CUtility.ServerPath(Path) + IO.Path.DirectorySeparatorChar + AssemblyName + ".dll"
                ' Load Assembly
                CLog.Sys("Loading Assembly {0} From {{{1}}}", AssemblyName, FullPath)

                Try
                    Dim MyAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(FullPath)
                    CLog.Sys("Assembly Loaded")
                    Dim Idx As Integer = 0
                    For Each Item In MyAssembly.GetTypes
                        Try
                            If (Not Item.GetInterfaces.Contains(GetType(IComponent))) OrElse Item.IsAbstract Then
                                Continue For
                            End If
                            CLog.Info("Find Component {0}", Item.FullName)
                            Dim Instance As IComponent = Activator.CreateInstance(Item)
                            Try
                                ComponentList.Add(AssemblyName.ToUpper + "." + Instance.Name.ToUpper, Instance.GetType)
                                CompList.Add(AssemblyName.ToUpper + "." + Instance.Name.ToUpper, Instance.GetType)
                                Idx += 1
                            Catch ex As Exception
                                CLog.Warning(ex.ToString)
                            End Try

                        Catch ex As Exception
                            CLog.Warning("Invalid Component : {0}", ex.Message)
                        End Try
                    Next
                    CLog.Info("Totally {0} Component(s) Added", Idx)
                    LoadedAssemblyList.Add(AssemblyName.ToUpper)
                Catch ex As Exception
                    CLog.Err("Load Component Fail : {0}", ex.Message)
                    Throw New CError.CBusinessException(CError.CErrorCode.LOAD_COMPONENT_FAIL, ex)
                End Try
            End SyncLock

            Return CompList
        End Function

        Public Shared Function ComponentLoader(ByVal Path As String, ByVal AssemblyName As String, ByVal ComponentName As String, ParamArray Interfaces As Type()) As Type
            'For Each Item In Interfaces
            '    If Not Item.IsInterface Then Throw New CError.CBusinessException(CError.CErrorCode.LOAD_COMPONENT_FAIL, "Validation Type Must Be Interface")
            'Next


            Dim Fullname As String = AssemblyName.ToUpper + "." + ComponentName.ToUpper
            If ComponentList.ContainsKey(Fullname) Then
                Return ComponentList.Item(Fullname)
            ElseIf LoadedAssemblyList.Contains(AssemblyName.ToUpper) Then
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_COMPONENT, _
                                                     "Can not Find Component {{{0}}} in Assembly {{{1}}}", AssemblyName, ComponentName)
            End If

            SyncLock SyncObject
                If ComponentList.ContainsKey(Fullname) Then
                    Return ComponentList.Item(Fullname)
                ElseIf LoadedAssemblyList.Contains(AssemblyName.ToUpper) Then
                    Throw New CError.CBusinessException(CError.CErrorCode.INVALID_COMPONENT, _
                                                         "Can not Find Component {{{0}}} in Assembly {{{1}}}", AssemblyName, ComponentName)
                End If
                ' Get Full Path
                Dim FullPath As String = CUtility.ServerPath(Path) + IO.Path.DirectorySeparatorChar + AssemblyName + ".dll"
                ' Load Assembly
                CLog.Sys("Loading Assembly {0} From {{{1}}}", AssemblyName, FullPath)

                Try
                    Dim MyAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(FullPath)
                    CLog.Sys("Assembly Loaded")
                    Dim Idx As Integer = 0
                    For Each Item In MyAssembly.GetTypes
                        Try
                            If (Not Item.GetInterfaces.Contains(GetType(IComponent))) OrElse Item.IsAbstract Then
                                Continue For
                            End If
                            CLog.Info("Find Component {0}", Item.FullName)
                            Dim Instance As IComponent = Activator.CreateInstance(Item)
                            Try
                                ComponentList.Add(AssemblyName.ToUpper + "." + Instance.Name.ToUpper, Instance.GetType)
                                Idx += 1
                            Catch ex As Exception
                                CLog.Warning(ex.ToString)
                            End Try

                        Catch ex As Exception
                            CLog.Warning("Invalid Component : {0}", ex.Message)
                        End Try
                    Next
                    CLog.Info("Totally {0} Component(s) Added", Idx)
                    LoadedAssemblyList.Add(AssemblyName.ToUpper)
                Catch ex As Exception
                    CLog.Err("Load Component Fail : {0}", ex.Message)
                    Throw New CError.CBusinessException(CError.CErrorCode.LOAD_COMPONENT_FAIL, ex)
                End Try
            End SyncLock

            If ComponentList.ContainsKey(Fullname) Then
                Return ComponentList.Item(Fullname)
            ElseIf LoadedAssemblyList.Contains(AssemblyName.ToUpper) Then
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_COMPONENT, _
                                                     "Can not Find Component {{{0}}} in Assembly {{{1}}}", AssemblyName, ComponentName)
            Else
                ' Can not be here
                Throw New CError.CBusinessException(CError.CErrorCode.LOAD_COMPONENT_FAIL, _
                                                     "Invalid Comonent")
            End If
        End Function

        Public Shared Function HexToString(Hex As Byte()) As String
            Dim Sb As New Text.StringBuilder
            For Each B As Byte In Hex
                Sb.Append(B.ToString("X2"))
            Next

            Return Sb.ToString
        End Function

    End Class
End Namespace