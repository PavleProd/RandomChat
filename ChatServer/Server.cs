using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using ChatServer.Model;
using ChatServer.Net;

namespace ChatServer
{
    class Server
    {
        public Server()
        {
            _users = [];
            _userWaitlistMap = [];
            _userWaitlist = [];
            _userConversationMap = [];

            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 24901);
        }
        public void Process()
        {
            try
            {
                _listener.Start();

                while (true)
                {
                    UserSocket userSocket;
                    try
                    {
                        userSocket = new UserSocket(_listener.AcceptTcpClient());
                    }
                    catch
                    {
                        continue;
                    }
                    
                    Task.Run(() => userSocket.ProcessPackets());

                    _users[userSocket.Id] = userSocket;

                    if (_userWaitlist.Count > 0)
                    {
                        StartChat(userSocket.Id, TakeUserFromWaitlist());
                    }
                    else
                    {
                        AddClientToWaitlist(userSocket.Id);
                    }


                    Console.WriteLine($"Broj Klijenata: {_users.Count}, Broj Klijenata na waitlisti: {_userWaitlist.Count}");
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
            var waitlistReference = _userWaitlist.AddLast(clientId);
            _userWaitlistMap[clientId] = waitlistReference;
        }

        public void DisconnectUser(Guid userId)
        {
            _users.Remove(userId);

            var inWaitlist = _userWaitlistMap.TryGetValue(userId, out var waitlistReference);
            if (inWaitlist && waitlistReference != null)
            {
                _userWaitlist.Remove(waitlistReference);
            }

            bool isLinked = _userConversationMap.TryGetValue(userId, out var connectedUserId);
            if (isLinked)
            {
                _userConversationMap.Remove(userId);
                _userConversationMap.Remove(connectedUserId);

                // send end chat signal to connected user
                Debug.Assert(_users.ContainsKey(connectedUserId), "Connected user is not in user collection!");
                var connectedUser = _users[connectedUserId];
                connectedUser.SendEndChat();
            }
            
        }

        public void ForwardMessage(Guid senderId, Message message)
        {
            var receiverId = _userConversationMap[senderId];
            var receiver = _users[receiverId];

            receiver.SendMessage(message);
        }

        private void StartChat(Guid clientId1, Guid clientId2)
        {
            _userConversationMap[clientId1] = clientId2;
            _userConversationMap[clientId2] = clientId1;

            var client1 = _users[clientId1];
            var client2 = _users[clientId2];

            client1.SendStartChat(client2.userData);
            client2.SendStartChat(client1.userData);
        }

        private Guid TakeUserFromWaitlist()
        {
            Debug.Assert(_userWaitlist.Count > 0);

            Guid waitingClientId = _userWaitlist.First();
            
            // remove from waitlist
            _userWaitlistMap.Remove(waitingClientId);
            _userWaitlist.RemoveFirst();

            return waitingClientId;
        }        

        private TcpListener _listener;
        private Dictionary<Guid, UserSocket> _users;
        private Dictionary<Guid, Guid> _userConversationMap;

        private Dictionary<Guid, LinkedListNode<Guid>> _userWaitlistMap;
        private LinkedList<Guid> _userWaitlist;
    }
}