using ChatServer.Common;
using ChatServer.Model;
using System.IO;
using System.Net.Sockets;

namespace ChatServer.Net.IO
{
    class PacketReader : BinaryReader
    {
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
        }
        public OperationCode ReadOpCode()
        {
            return (OperationCode)_networkStream.ReadByte();
        }

        public Message ReadMessage()
        {
            string message = ReadField().ToString();
            TimeOnly time = ReadField().ToTimeOnly();
            return new Message(message, time);
        }

        public User ReadUser()
        {
            string username = ReadField().ToString();
            return new User(username);
        }

        private PacketField ReadField()
        {
            byte[] messageBuffer;
            var fieldLength = ReadInt32();
            messageBuffer = new byte[fieldLength];

            _networkStream.Read(messageBuffer, 0, fieldLength);
            return new PacketField(messageBuffer);
        }

        private NetworkStream _networkStream;
    }
}
