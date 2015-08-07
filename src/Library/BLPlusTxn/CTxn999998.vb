Imports ServerPlatform.Library.Data

Public Class CTxn999998
    Inherits TransactionBase

    Public Overrides Sub Process(DatabaseFactory As Library.Utility.CDatabaseFactory)
        _Response.SystemList.Add(New CTxnResp999998.CSystemList("BLPLUS", "Bancslink+", "Images/Bancslink.fw.png", "http://BLPlus/Index.html"))
        _Response.SystemList.Add(New CTxnResp999998.CSystemList("BLMornitor", "Bancslink Mornitoring System", "Images/Icon_Debug.png", "#"))
        _Response.SystemList.Add(New CTxnResp999998.CSystemList("BLReporting", "BANCSLINK+ Reporting System", "Images/Icon_Reporting.png", "#"))
        _Response.SystemList.Add(New CTxnResp999998.CSystemList("BLEnterpriseOA", "BANCSLINK+ Enterprise OA", "Images/Icon_Management.png", "#"))
    End Sub

    Private _Response As New CTxnResp999998
    Public Overrides ReadOnly Property Response As Application.CTransactionBase.CStandardResponseBase
        Get
            Return _Response
        End Get
    End Property
    Public Overrides ReadOnly Property TxnCode As String
        Get
            Return "999998"
        End Get
    End Property

    Public Class CTxnResp999998
        Inherits BancslinkTxnBase.BancslinkTxnResponseBase

        Public Class CSystemList
            Sub New(SystemId As String, SystemName As String, SystemIcon As String, SystemLink As String)
                Me.SystemIcon = SystemIcon
                Me.SystemLink = SystemLink
                Me.SystemId = SystemId
                Me.SystemName = SystemName
            End Sub
            Property SystemName As String
            Property SystemId As String
            Property SystemIcon As String
            Property SystemLink As String
        End Class
        Property SystemList As New List(Of CSystemList)

        Public Overrides Function Encode() As Library.Data.CMessage
            Dim Message As CMessage = MyBase.Encode()
            Message.SetValue("SystemList", SystemList)

            Return Message
        End Function


    End Class
End Class

