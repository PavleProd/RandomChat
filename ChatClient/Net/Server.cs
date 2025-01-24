﻿using ChatClient.Common;
using ChatClient.MVVM.Model;
using ChatClient.Net.IO;
using System.Net.Sockets;

namespace ChatClient.Net
{
    class Server
    {
        public void Connect(User user)
        {
            if (!IsConnected())
            {
                _client = new TcpClient();
                _client.Connect("127.0.0.1", 24901);
                _packetReader = new PacketReader(_client.GetStream());

                _userData = user;
                SendInitDataPacket();

                Task.Run(() => ReadPackets());
            }
        }

        public bool IsConnected()
        {
            return _client != null && _client.Connected;
        }

        public void Disconnect()
        {
            if (IsConnected())
            {
                _client.Client.Shutdown(SocketShutdown.Both);
                _client.Client.Close();
                _client.Close();
            }            
        }

        public void ReadPackets()
        {            
            try
            {
                while (true)
                {
                    var opCode = _packetReader.ReadOpCode();
                    switch (opCode)
                    {
                        case OperationCode.LinkClients:
                            {
                                break;
                            }
                        case OperationCode.Message:                        
                            {
                                ReceiveMessage();
                                break;
                            }
                        case OperationCode.EndClientsLink:
                            {
                                Disconnect();
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("ERROR: wrong operation code!");
                                break;
                            }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Konekcija sa serverom prekinuta!");
                Disconnect();  
            }
        }

        public void SendMessage(Message message)
        {
            PacketBuilder builder = new PacketBuilder();
            builder.WriteMessage(message);
            SendPacket(builder);
        }

        public void ReceiveMessage()
        {
            Message message = _packetReader.ReadMessage();
            MessageReceived?.Invoke(message);
        }

        private void SendPacket(PacketBuilder builder)
        {
            if (IsConnected())
            {
                _client.Client.Send(builder.GetRawData());
            }
        }            

        private void SendInitDataPacket()
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteInitData(_userData);
            SendPacket(packetBuilder);
        }

        public event Action<Message> MessageReceived;

        private TcpClient? _client;
        private PacketReader _packetReader;
        private User _userData;
    }
}
