using ChatServer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    class PacketBuilder
    {
        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }

        public void WriteOpCode(ServerToClientOperations opcode)
        {
            _memoryStream.WriteByte((byte)opcode);
        }

        public void WriteMessage(Message message)
        {
            WriteOpCode(ServerToClientOperations.Message);
            WriteString(message.AuthorId);
            WriteString(message.Text);
        }

        private void WriteString(string message)
        {
            var messageLength = message.Length;
            _memoryStream.Write(BitConverter.GetBytes(messageLength));
            _memoryStream.Write(Encoding.ASCII.GetBytes(message));
        }

        private MemoryStream _memoryStream;
    }
}
