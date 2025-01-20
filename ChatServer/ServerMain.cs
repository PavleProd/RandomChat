using System.Net;
using System.Net.Sockets;
using ChatServer.Net;

namespace ChatServer
{
    class ServerMain
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

                // TODO:
                // uparivanje sa drugim klijentima
                // izbacivanje prilikom diskonekcije
                
            }
        }

        private static TcpListener _listener;
        private static List<Client> _clients;
    }
}