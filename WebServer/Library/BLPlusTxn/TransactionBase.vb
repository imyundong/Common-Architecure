Imports ServerPlatform.Library.Workflow
Imports ServerPlatform.Library.Utility
Imports ServerPlatform.Application
Imports ServerPlatform.Library.Data
Imports System.Threading.Tasks

Public MustInherit Class TransactionBase
    Inherits BancslinkTxnBase
    Private Function SupervisorOverrideCheck(ByVal TxnCode As String, ByVal BranchCategory As Integer, ByVal TellerRole As List(Of CUserRoleCategory)) As HashSet(Of String)
        Dim OverrideCheckList As New HashSet(Of String)
        Dim OverrideCount As Integer = 0
        For Each Item As COverrideList In OverrideList
            For Each Role As CUserRoleCategory In TellerRole
                If Item.Match(TxnCode, BranchCategory, Role.RoleId) = True Then
                    CLog.Info("Override Code {0} Found", Item.OverrideCode)
                    If OverrideCode.ContainsKey(Item.OverrideCode) Then
                        OverrideCheckList.Add(Item.OverrideCode)
                        OverrideCount += 1

                        Exit For
                    Else
                        CLog.Warning("Override Code {0} is Not Defined", Item.OverrideCode)
                    End If
                End If
            Next
        Next

        Return OverrideCheckList
    End Function
    Public Overrides Sub PreProcess(DatabaseFactory As CDatabaseFactory)
        MyBase.PreProcess(DatabaseFactory)
        ' Retrieve User Info, Check Token(TODO)
        UserInfo = New CSystemUser
        UserInfo.Search(DatabaseFactory, UserId, Nothing)

        Dim UserBranch As New CBranch
        UserBranch.Search(DatabaseFactory, UserInfo.Branch, Nothing)

        ' Check Supervisor
        If SupervisorId = 0 Then

            ' Check Supervisor Override
            Dim OvCheckList As HashSet(Of String) = SupervisorOverrideCheck(ClientTxnCode, UserBranch.BranchCategory, GetUserRoles(DatabaseFactory, UserId))
            Dim OvList As New List(Of String)
            If OvCheckList.Count > 0 Then
                ' Prepare Screen Data
                Dim ScreenData As New Generic.Dictionary(Of String, String)
                For i As Integer = 0 To Message.Keys.Count - 1
                    ScreenData.Add(Message.Keys(i), Message.Values(i))
                Next

                Dim Tasks(OvCheckList.Count - 1) As Task(Of String)
                Dim j As Integer = 0
                For Each Code As String In OvCheckList
                    Tasks(j) = OverrideCheckAsync(Code, ScreenData, Nothing)
                    j += 1
                Next
                ' Wait All Validation Finished
                For Each Task As Task(Of String) In Tasks
                    Dim Result As String = Task.Result
                    If Not String.IsNullOrEmpty(Result) Then
                        OvList.Add(Result)
                    End If
                Next
            End If

            If OvList.Count > 0 Then
                For Each Item As String In OvList
                    Dim Override As COverrideCode = OverrideCode.Item(Item)
                    DirectCast(Response, BancslinkTxnResponseBase).OverrideDetails.Add(New BancslinkTxnResponseBase.COverrideDetail _
                                                                                       With {.Capability = Override.Capability, .OverrideCode = Override.Code, .OverrideDescription = Override.OverrideDescription})
                Next

                DirectCast(Response, BancslinkTxnResponseBase).Action = BancslinkTxnResponseBase.CAction.SUP
                Throw New CError.CBusinessException(CError.CErrorCode.SUPERVISOR_OVERRIDE_REQUIRED, "Supervisor Override Required")
            End If
        Else
            ' Check Supervisor
            If (Not String.IsNullOrEmpty(SupervisorToken)) AndAlso CToken.Tokens.ContainsKey(SupervisorToken) Then
                If CToken.Tokens.Item(SupervisorToken).ExpiryDate < Now Then
                    Throw New CError.CBusinessException(CError.CErrorCode.USER_TOKEN_EXPIRED, "Supervisor Token Expired")
                End If
                CToken.Tokens.Remove(SupervisorToken)
            Else
                Throw New CError.CBusinessException(CError.CErrorCode.INVALID_USER_TOKEN, "Invalid Supervisor Token")
            End If
        End If

    End Sub

    Public MustOverride Overrides Sub Process(DatabaseFactory As CDatabaseFactory)
    Public MustOverride Overrides ReadOnly Property TxnCode As String
End Class
