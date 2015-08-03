Module Module1

    Sub Main()
        Dim Listener As New Net.Sockets.TcpListener(New System.Net.IPEndPoint(Net.IPAddress.Any, 9999))
        Listener.Start()
        Dim Client As Net.Sockets.TcpClient = Listener.AcceptTcpClient

        While True
            Dim Len(4) As Byte
            Client.Client.Receive(Len)
            Dim MsgLen As Integer = Text.Encoding.ASCII().GetString(Len).Trim

            Dim Buffer(MsgLen - 1) As Byte
            Dim Size As Integer = Client.Client.Receive(Buffer)

            Dim Content As String = Text.Encoding.GetEncoding(950).GetString(Buffer)
            Dim TxnCode As String = Content.Substring(49, 6)
            Console.WriteLine("Rx : " + Content.TrimEnd(" "c))

            Try
                Using Sr As New IO.StreamReader(TxnCode + ".xml")
                    Dim XmlSerilizer As New Xml.Serialization.XmlSerializer(GetType(Message))
                    Dim Msg As Message = XmlSerilizer.Deserialize(Sr)
                    Client.Client.Send(Text.Encoding.GetEncoding(950).GetBytes(Msg.Content))
                    Console.WriteLine("Tx : " + Msg.Content)
                End Using

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        End While

    End Sub

End Module
