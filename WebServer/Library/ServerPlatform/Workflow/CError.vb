Imports System.Runtime.Serialization
Imports System.Xml.Serialization
Imports ServerPlatform.Library.Utility

Namespace Workflow
    Public Class CError
        <Serializable>
        Public Enum CErrorCode As Integer
            SUCCESSFUL = 0
            SYSTEM_START_FAIL = 1
            INVALID_REQUEST = 2
            INVALID_WORKFLOW = 3
            INVALID_WORKFLOW_NODE = 4
            INVALID_WORKFLOW_COMPONENT = 5
            BUSINESS_ERROR = 6
            INVALID_COMPONENT = 7
            LOAD_COMPONENT_FAIL = 8
            COMMUNICATION_NOT_INITIALISED = 9
            INVALID_MESSAGE_ID = 10
            MESSAGE_TIME_OUT = 11
            INVALID_APPLICATION_CATEGORY = 12
            INVALID_PLATFORM_MODE = 13
            INVALID_COMMUNICATION_COMPONENT = 14
            CONNECTION_DISCONNECTED = 15
            DB_CONNECTION_FAIL = 16
            DATABASE_QUERY_FAIL = 17
            RECORD_NOT_FOUND = 18
            INVALID_TRANSACTION_CODE = 19
            LOAD_TXN_COMPONENT_FAIL = 20
            TXN_COMPONENT_NOT_FOUND = 21
            SYSTEM_IS_STARTING = 22
            SYSTEM_ALREADY_STARTED = 23
            INVALID_APPLICATION_STATUS = 24
            APPLICATION_PLATFORM_START_FAIL = 25
            INVALID_TRANSACTION_PARAMETER = 26
            DATABASE_INSERT_FAIL = 27
            DATABASE_UPDATE_FAIL = 28
            DATABASE_DELETE_FAIL = 29
            INVALID_DATABASE_CLASS = 30
            INVALID_PAGE_ID = 31
            INVALID_USER_ID = 32
            REACH_MAX_CONNECTIONS = 33
            FIELD_FORMAT_ERROR = 34
            TXN_FAIL = 35
            MESSAGE_NUMBERS_EXCCEED = 36

            ' Bancslink Transaction
            INVALID_TELLER = 500
            INVALID_USER_OR_PASSWORD = 501
            TXN_PARAMETER_NOT_DEFINED = 502
            SUPERVISOR_OVERRIDE_REQUIRED = 503
            INVALID_USER_TOKEN = 504
            USER_TOKEN_EXPIRED = 505

            INTERNAL_ERROR = 9999

        End Enum

        ''' <summary>
        ''' Display Error Message
        ''' </summary>
        ''' <remarks></remarks>
        <Serializable>
        Public Class CBusinessException
            Inherits Exception

            Public Overrides ReadOnly Property Message As String
                Get
                    Return _ErrDescription
                End Get
            End Property
            Property ErrCode As CErrorCode = CErrorCode.SUCCESSFUL
            Property ErrDescription As String = ""
            Property SystemId As String

            Public Sub New(ErrCode As CErrorCode, Exception As Exception)
                MyBase.New(Exception.Message, Exception)
                Me.ErrCode = ErrCode
                Me.ErrDescription = Exception.Message
            End Sub

            Public Sub New(SystemId As String, ErrCode As CErrorCode, Exception As Exception)
                MyBase.New(ErrCode, Exception)
                Me.SystemId = SystemId
            End Sub

            Public Sub New(SystemId As String, ErrCode As CErrorCode, Message As String, ByVal ParamArray Param As Object())
                Me.New(ErrCode, Message, Param)
                Me.SystemId = SystemId
            End Sub

            Public Sub New(ErrCode As CErrorCode, Message As String, ByVal ParamArray Param As Object())
                MyBase.New(String.Format(Message, Param))
                Me.ErrCode = ErrCode
                ErrDescription = String.Format(Message, Param)
            End Sub

            Protected Sub New(Info As System.Runtime.Serialization.SerializationInfo, Context As System.Runtime.Serialization.StreamingContext)
                ' TODO, For Serilization
                MyBase.New(Info, Context)
            End Sub
        End Class
    End Class
End Namespace