using ChatServer.Common;
using ChatServer.Model;
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
            WriteString(message.Text);
            WriteTime(message.SendingTime);
        }

        public void WriteStartChat(User user)
        {
            WriteOpCode(OperationCode.StartChat);
            WriteUser(user);
        }

        public void WriteEndChat()
        {
            WriteOpCode(OperationCode.EndChat);
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

        private void WriteTime(TimeOnly time)
        {
            WriteString(time.ToString());
        }

        private void WriteUser(User user)
        {
            WriteString(user.Username);
        }

        private readonly MemoryStream _memoryStream;
    }
}
