using ChatServer.Common;
using System.Text;

namespace ChatServer.Net.IO
{
    class PacketBuilder
    {
        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }
        public byte[] GetRawData()
        {
            return _memoryStream.GetBuffer();
        }

        public void WriteMessage(Message message)
        {
            WriteOpCode(OperationCode.Message);
            WriteString(message.AuthorId);
            WriteString(message.Text);
        }

        private void WriteOpCode(OperationCode opcode)
        {
            _memoryStream.WriteByte((byte)opcode);
        }

        private void WriteString(string message)
        {
            var messageLength = message.Length;
            _memoryStream.Write(BitConverter.GetBytes(messageLength));
            _memoryStream.Write(Encoding.ASCII.GetBytes(message));
        }

        private readonly MemoryStream _memoryStream;
    }
}
