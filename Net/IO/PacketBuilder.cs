using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    class PacketBuilder
    {
        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _memoryStream.WriteByte(opcode);
        }

        public void WriteString(string message)
        {
            var messageLength = message.Length;
            _memoryStream.Write(BitConverter.GetBytes(messageLength));
            _memoryStream.Write(Encoding.ASCII.GetBytes(message));
        }

        public byte[] GetPacketBytes()
        {
            return _memoryStream.GetBuffer();
        }

        private MemoryStream _memoryStream;
    }
}
