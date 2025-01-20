using ChatServer.Common;
using System.Net.Sockets;
using System.Text;

namespace ChatServer.Net.IO
{
    class PacketReader : BinaryReader
    {
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
        }

        public ClientToServerOperations ReadOpCode()
        {
            return (ClientToServerOperations)_networkStream.ReadByte();
        }

        public Message ReadMessage()
        {
            string id = ToString(ReadField());
            string message = ToString(ReadField());

            return new Message(id, message);
        }

        public string ReadUsername()
        {
            return ToString(ReadField());
        }

        private byte[] ReadField()
        {
            byte[] messageBuffer;
            var fieldLength = ReadInt32();
            messageBuffer = new byte[fieldLength];

            _networkStream.Read(messageBuffer, 0, fieldLength);
            return messageBuffer;
        }

        private int ToInt(byte[] field)
        {
            return BitConverter.ToInt32(field, 0);
        }

        private string ToString(byte[] field)
        {
            return Encoding.ASCII.GetString(field);
        }

        private NetworkStream _networkStream;
    }
}
