using ChatServer.Common;
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
            string id = ReadField().ToString();
            string message = ReadField().ToString();

            return new Message(id, message);
        }

        public string ReadUsername()
        {
            return ReadField().ToString();
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
