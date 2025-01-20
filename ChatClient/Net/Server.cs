using ChatClient.Common;
using ChatClient.Net.IO;
using System.Net.Sockets;

namespace ChatClient.Net
{
    class Server
    {
        public Server()
        {   
            _client = new TcpClient();
        }

        public void Connect(string username)
        {
            if (!IsConnected())
            {
                _client.Connect("127.0.0.1", 24901);
                _packetReader = new PacketReader(_client.GetStream());

                SendInitDataPacket(username);

                Task.Run(() => ReadPackets());
            }
        }

        public bool IsConnected()
        {
            return _client.Connected;
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public void ReadPackets()
        {            
            try
            {
                while (true)
                {
                    var opCode = _packetReader.ReadOpCode();
                    switch (opCode)
                    {
                        case OperationCode.Message:
                            {
                                ReceiveMessage();
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("ERROR: wrong operation code!");
                                break;
                            }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Konekcija sa serverom prekinuta!");
                Disconnect();  
            }
        }

        public void ReceiveMessage()
        {
            Message message = _packetReader.ReadMessage();
            MessageReceived?.Invoke(message);
        }

        public void SendPacket(PacketBuilder builder)
        {
            _client.Client.Send(builder.GetRawData());
        }            

        private void SendInitDataPacket(string username)
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteInitData(username);
            SendPacket(packetBuilder);
        }

        public event Action<Message> MessageReceived;

        private TcpClient _client;
        private PacketReader _packetReader;
    }
}
