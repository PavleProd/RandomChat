using System.Net;
using System.Net.Sockets;
using ChatServer.Common;
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
            _linkedClients = [];

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

                    if (_clientWaitlist.Count > 0)
                    {
                        LinkClients(client.Id, TakeClientFromWaitlist());
                    }
                    else
                    {
                        AddClientToWaitlist(client.Id);
                    }

                    /*
                        // Test Message
                        PacketBuilder builder = new PacketBuilder();
                        builder.WriteMessage(new Common.Message("123", "Uspesno uspostavljena konekcija!"));
                        client.ClientSocket.Client.Send(builder.GetRawData());
                    */


                    Console.WriteLine($"Broj Klijenata: {_clients.Count}, Broj Klijenata na waitlisti: {_clientWaitlist.Count}");
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
            var _disconnectingClient = _clients[clientId];
            _clients.Remove(clientId);
            var inWaitlist = _clientWaitlistReferences.TryGetValue(clientId, out var waitlistReference);
            if (inWaitlist && waitlistReference != null)
            {
                _clientWaitlist.Remove(waitlistReference);
            }

            bool isLinked = _linkedClients.TryGetValue(clientId, out var linkedClientId);
            if (isLinked)
            {
                // remove links between clients
                _linkedClients.Remove(clientId);
                _linkedClients.Remove(linkedClientId);

                // send message to other client that first client was disconnected

                // TODO: ocekivano da je u recniku
                var linkedClient = _clients[linkedClientId];
                linkedClient.SendEndLinkMessage(_disconnectingClient.Username);
            }
            
        }

        public void ForwardMessage(Guid senderId, Message message)
        {
            var receiverId = _linkedClients[senderId];
            var receiver = _clients[receiverId];

            receiver.SendMessage(message);
        }

        private void LinkClients(Guid clientId1, Guid clientId2)
        {
            _linkedClients[clientId1] = clientId2;
            _linkedClients[clientId2] = clientId1;

            var client1 = _clients[clientId1];
            var client2 = _clients[clientId2];

            client1.SendEstablishLinkMessage(client2.Username);
            client2.SendEstablishLinkMessage(client1.Username);
        }

        private Guid TakeClientFromWaitlist()
        {
            // TODO: dodaj da je obavezno da waitlista nije prazna

            Guid waitingClientId = _clientWaitlist.First();
            
            // remove from waitlist
            _clientWaitlistReferences.Remove(waitingClientId);
            _clientWaitlist.RemoveFirst();

            return waitingClientId;
        }        

        private TcpListener _listener;
        private Dictionary<Guid, Client> _clients;
        private Dictionary<Guid, Guid> _linkedClients;

        private Dictionary<Guid, LinkedListNode<Guid>> _clientWaitlistReferences;
        private LinkedList<Guid> _clientWaitlist;
    }
}