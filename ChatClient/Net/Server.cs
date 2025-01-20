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
                _client.Connect("127.0.0.1", 7891);
                _packetReader = new PacketReader(_client.GetStream());

                SendInitDataPackage(username);

                Task.Run(() => ReadPackets());
            }
        }

        public bool IsConnected()
        {
            return _client.Connected;
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

        public void Disconnect()
        {
            _client.Close();
        }

        private void SendInitDataPackage(string username)
        {
            var connectPacket = new PacketBuilder();
            connectPacket.WriteOpCode((byte)OperationCode.InitData);
            connectPacket.WriteString(username);
            _client.Client.Send(connectPacket.GetPacketBytes());
        }

        public event Action<Message> MessageReceived;

        private TcpClient _client;
        private PacketReader _packetReader;
    }
}
