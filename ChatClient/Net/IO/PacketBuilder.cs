using ChatClient.Common;
using System.IO;
using System.Text;

namespace ChatClient.Net.IO
{
    class PacketBuilder
    {
        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }
        public byte[] GetRawPacket()
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
