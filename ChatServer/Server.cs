using System.Net;
using System.Net.Sockets;
using ChatServer.Net;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Server
    {
        public Server()
        {
            _clients = [];
            _clientWaitlistReferences = [];
            _clientWaitlist = [];

            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 24901);
        }
        public void Process()
        {
            try
            {
                _listener.Start();

                while (true)
                {
                    var client = new Client(_listener.AcceptTcpClient());
                    _clients[client.Id] = client;

                    AddClientToWaitlist(client.Id); // trenutno dodaj sve na waitlistu

                    PacketBuilder builder = new PacketBuilder();
                    builder.WriteMessage(new Common.Message("123", "Uspesno uspostavljena konekcija!"));
                    client.ClientSocket.Client.Send(builder.GetRawData());

                    // TODO:
                    // uparivanje sa drugim klijentima
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL SERVER ERROR!");
                Console.WriteLine(ex.ToString());
            }            
        }

        public void AddClientToWaitlist(Guid clientId)
        {
            // TODO dodaj logiku za proveru da li je vec na listi cekanja

            var waitlistReference = _clientWaitlist.AddLast(clientId);
            _clientWaitlistReferences[clientId] = waitlistReference;
        }

        public void DisconnectClient(Guid clientId)
        {
            _clients.Remove(clientId);
            _clientWaitlistReferences.TryGetValue(clientId, out var waitlistReference);
            if (waitlistReference != null)
            {
                _clientWaitlist.Remove(waitlistReference);
            }

            Console.WriteLine($"Broj Klijenata: {_clients.Count}, Broj Klijenata na waitlisti: {_clients.Count}");
        }        

        private TcpListener _listener;
        private Dictionary<Guid, Client> _clients;
        private Dictionary<Guid, LinkedListNode<Guid>> _clientWaitlistReferences;
        private LinkedList<Guid> _clientWaitlist;
    }
}