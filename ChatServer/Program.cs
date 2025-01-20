using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            _clients = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);

            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _clients.Add(client);

                client.WriteMessage(new Common.Message("123", "Konekcija uspesno uspostavljena!"));
            }
        }

        private static TcpListener _listener;
        private static List<Client> _clients;
    }
}