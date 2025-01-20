using System.Text;

namespace ChatServer.Net.IO
{
    internal readonly struct PacketField
    {
        public PacketField(byte[] data)
        {
            _data = data;
        }
        public int ToInt32()
        {
            return BitConverter.ToInt32(_data, 0);
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(_data);
        }

        private readonly byte[] _data;
    }
}
