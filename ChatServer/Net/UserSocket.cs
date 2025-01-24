using ChatServer.Common;
using ChatServer.Model;
using ChatServer.Net.IO;
using System.Net.Sockets;

namespace ChatServer.Net
{
    class UserSocket
    {
        public UserSocket(TcpClient socket)
        {
            Socket = socket;
            Id = Guid.NewGuid();

            _packetReader = new PacketReader(Socket.GetStream());
            LoadInitData();
        }

        public void LoadInitData()
        {
            var opCode = _packetReader.ReadOpCode();

            if (opCode != OperationCode.InitData)
            {
                throw new Exception();
            }

            userData = _packetReader.ReadUser();
            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {userData.Username}");
        }

        public void SendMessage(Message message)
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteMessage(message);
            SendPacket(packetBuilder);
        }

        public void SendStartChat(User user)
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteStartChat(user);
            SendPacket(packetBuilder);
        }

        public void SendEndChat()
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteEndChat();
            SendPacket(packetBuilder);
        }

        private void SendPacket(PacketBuilder packetBuilder)
        {
            Socket.Client.Send(packetBuilder.GetRawData());
        }

        public void ProcessPackets()
        {            
            try
            {               
                while (true)
                {
                    var opCode = _packetReader.ReadOpCode();
                    switch (opCode)
                    {
                        case OperationCode.None:
                            {
                                break;
                            }
                        case OperationCode.Message:
                            {
                                Message message = _packetReader.ReadMessage();
                                Program.RandomChat.ForwardMessage(Id, message);
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

                Program.RandomChat.DisconnectUser(Id);
                Socket.Close();
            }
        }

        public Guid Id { get; }
        public TcpClient Socket { get; }

        public User userData { get; private set;  }

        private readonly PacketReader _packetReader;
    }
}
