﻿using ChatServer.Common;
using ChatServer.Net.IO;
using System.Net.Sockets;

namespace ChatServer
{
    class Client
    {
        public Client(TcpClient client)
        {
            ClientSocket = client;
            Id = Guid.NewGuid();

            _packetReader = new PacketReader(ClientSocket.GetStream());

            ReadUsername();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");
        }

        public void ReadUsername()
        {
            var packetReader = new PacketReader(ClientSocket.GetStream());
            
            var opCode = packetReader.ReadByte();

            if ((ClientToServerOperations)opCode == ClientToServerOperations.InitData)
            {
                Username = packetReader.ReadUsername();
            }

            // TODO: baci izuzetak ako se username ne prosledi pravilno
        }

        public void WriteMessage(Message message)
        {
            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteMessage(message);
            ClientSocket.Client.Send(packetBuilder.GetPacketBytes());
        }

        public string Username { get; set; }
        public Guid Id { get; }
        public TcpClient ClientSocket { get; }

        private readonly PacketReader _packetReader;
    }
}
