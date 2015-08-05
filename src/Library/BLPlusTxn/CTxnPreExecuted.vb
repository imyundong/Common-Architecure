Imports ServerPlatform.Library.Data
Imports ServerPlatform.Application
Imports ServerPlatform.Library.Utility
Public Class CTxnPreExecuted
    Inherits CTxnPreExecution

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        Dim SystemParameters As New List(Of CTxnParameter)
        ' Retrieve System Parameters From Database
        CLog.Info("Retrieve Transaction Parameters")

        CTxnParameter.SearchAll(DatabaseFactory, SystemParameters)
        CLog.Info("Totally {0} Records Found", SystemParameters.Count)
        For Each Parameter As CTxnParameter In SystemParameters
            BancslinkTxnBase.TxnParameters.Add((Parameter.TxnCode).ToUpper, Parameter)
        Next

        ' Retrieve User Role Category
        CLog.Info("Retrieve User Roles")
        Dim UserRoleList As New List(Of CUserRoleCategory)
        CUserRoleCategory.SearchAll(DatabaseFactory, UserRoleList)
        CLog.Info("Totally {0} Records Found", UserRoleList.Count)
        For Each UserRole As CUserRoleCategory In UserRoleList
            BancslinkTxnBase.UserRoles.Add(UserRole.RoleId, UserRole)
        Next

        ' Retrieve Override Code
        CLog.Info("Retrieve Override Code")
        Dim OverrideCodes As New List(Of COverrideCode)
        COverrideCode.SearchAll(DatabaseFactory, OverrideCodes)
        CLog.Info("Totally {0} Override Codes Found")
        For Each Item As COverrideCode In OverrideCodes
            BancslinkTxnBase.OverrideCode.Add(Item.Code, Item)
            CLog.Debug(Item.Code, Item.OverrideDescription)
        Next
        ' Dynamic Complie Override Code
        DynamicCompileOverrideCode()
        CLog.Info("Retrieve Override List")
        COverrideList.SearchAll(DatabaseFactory, BancslinkTxnBase.OverrideList)
        CLog.Info("Totally {0} Records Found in Override List", BancslinkTxnBase.OverrideList.Count)
        ' Load Override Status
        Dim OverrideStatusList As New List(Of COverrideStatus)
        COverrideStatus.SearchAll(DatabaseFactory, OverrideStatusList)
        For Each OverrideStatus As COverrideStatus In OverrideStatusList
            BancslinkTxnBase.OverrideStatus.Add(OverrideStatus.OverrideStatusId, OverrideStatus)
        Next
    End Sub

    Private Sub DynamicCompileOverrideCode()
        CLog.Info("Start to Complie Override Code")
        ' Dynamic Compile Code
        Try
            Dim Sb As New Text.StringBuilder
            For Each Item As Generic.KeyValuePair(Of String, COverrideCode) In BancslinkTxnBase.OverrideCode
                If Not String.IsNullOrEmpty(Item.Value.Code) AndAlso Item.Value.Code.ToUpper = "CUSTOMIZED" Then
                    Continue For
                End If
                Sb.AppendLine("Public Class " + Item.Key)
                Sb.AppendLine("    Implements " + GetType(IOverrideCheck).FullName)
                Sb.AppendLine("    Function Check(Screen As System.Collections.Generic.Dictionary(Of String, String), User As System.Collections.Generic.Dictionary(Of String, String)) As Boolean Implements " + GetType(IOverrideCheck).FullName + ".Check")
                If Not String.IsNullOrEmpty(Item.Value.Condition) Then
                    Sb.AppendLine(Item.Value.Condition)
                Else
                    Sb.AppendLine("        Return True")
                End If

                Sb.AppendLine("    End Function")
                Sb.AppendLine("End Class")
            Next

            Dim ComplierParams As New System.CodeDom.Compiler.CompilerParameters
            ComplierParams.GenerateInMemory = True
            ComplierParams.TreatWarningsAsErrors = False
            ComplierParams.ReferencedAssemblies.Add(GetType(IOverrideCheck).Assembly.Location)

            Dim Codes(0) As String
            Codes(0) = Sb.ToString

            Dim Provider As New VBCodeProvider
            Dim Result As System.CodeDom.Compiler.CompilerResults = Provider.CompileAssemblyFromSource(ComplierParams, Codes)

            If Result.Errors.Count > 0 Then
                For Each Item As CodeDom.Compiler.CompilerError In Result.Errors
                    CLog.Err(Item.ErrorText)
                Next

                Throw New Exception("Invalid Condition Code")
            End If

            ' Keep The Override Types
            For Each OverrideType As Type In Result.CompiledAssembly.GetTypes()
                If BancslinkTxnBase.OverrideCode.ContainsKey(OverrideType.FullName) Then
                    BancslinkTxnBase.OverrideCode.Item(OverrideType.FullName).OverrideType = OverrideType
                End If
            Next

            For Each Item As Generic.KeyValuePair(Of String, COverrideCode) In BancslinkTxnBase.OverrideCode
                If Not String.IsNullOrEmpty(Item.Value.Code) AndAlso Item.Value.Code.ToUpper = "CUSTOMIZED" Then
                    Item.Value.OverrideType = Type.GetType("ServerPlatform.Transaction.StandardApplication.BLPlusAppServerTxn." + Item.Value.Code)
                End If
            Next

        Catch ex As Exception
            CLog.Err("Compile Error : {0}", ex.ToString)
            Throw ex
        End Try
        CLog.Info("Compile Successful")
    End Sub

    Public Overrides ReadOnly Property Response As CTransactionBase.CStandardResponseBase
        Get
            Return Nothing
        End Get
    End Property
End Class

