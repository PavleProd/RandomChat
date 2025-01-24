using ChatClient.Common;
using ChatClient.MVVM.Model;
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

        public void WriteInitData(User user)
        {
            WriteOpCode(OperationCode.InitData);
            WriteString(user.Username);
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

        private readonly MemoryStream _memoryStream;
    }
}
