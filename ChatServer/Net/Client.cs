using ChatServer.Common;
using ChatServer.Net.IO;
using System.Net.Sockets;

namespace ChatServer.Net
{
    class Client
    {
        public Client(TcpClient client)
        {
            ClientSocket = client;
            Id = Guid.NewGuid();

            _packetReader = new PacketReader(ClientSocket.GetStream());       

            Task.Run(() => ProcessPackets());
        }

        public void ReadInitData()
        {
            var opCode = _packetReader.ReadOpCode();

            if (opCode != OperationCode.InitData)
            {
                throw new Exception("INVALID OPERATION CODE");
            }

            Username = _packetReader.ReadUsername();
            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");
        }

        public void SendMessage(Message message)
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteMessage(message);
            ClientSocket.Client.Send(packetBuilder.GetRawPacket());
        }

        public void ProcessPackets()
        {            
            try
            {
                ReadInitData();

                while (true)
                {
                    var opCode = _packetReader.ReadOpCode();
                    switch (opCode)
                    {
                        case OperationCode.Message:
                            {
                                break;
                            }
                        default:
                            {
                                throw new Exception("INVALID OPERATION CODE");
                            }
                    }
                }
            }
            catch
            {
                Console.WriteLine($"[{Id}]: Disconnected!");
                ClientSocket.Close();
            }
        }

        public string? Username { get; set; }
        public Guid Id { get; }
        public TcpClient ClientSocket { get; }

        private readonly PacketReader _packetReader;
    }
}
