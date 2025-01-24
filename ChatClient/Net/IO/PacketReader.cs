using ChatClient.Common;
using ChatClient.MVVM.Model;
using System.IO;
using System.Net.Sockets;

namespace ChatClient.Net.IO
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
            return new Message(message, MessageType.Incoming, time);
        }

        public User ReadUser()
        {
            return new User(ReadField().ToString());
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
