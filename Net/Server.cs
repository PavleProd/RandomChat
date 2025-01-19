using ChatClient.Net.IO;
using System;
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
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteString(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
        }

        private TcpClient _client;
    }
}
