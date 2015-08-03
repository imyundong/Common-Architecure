Imports ServerPlatform.Library.Data
Namespace Utility
    ''' <summary>
    ''' Communcation Component Interface
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICommunication
        Function Send(ByVal Message As CMessage) As Boolean
        Function Receive(ByVal MessageId As String) As CMessage
        Sub BeginReceive()

        Event MessageReceived(ByVal Message As CMessage)
        Sub Init(ByVal CommInfo As CCommInfo.CHostInfo)
        Sub Close()
        Sub StopServices()

    End Interface

    Public Interface IDataConverter
        Property ConverterInfo As CCommInfo.CConverterInfo
        Function Serialise(ByVal Message As CMessage) As IO.MemoryStream
        Function Deserialise(ByRef Buffer() As Byte) As CMessage
    End Interface
End Namespace