using ChatClient.Common;
using ChatClient.MVVM.Model;
using ChatClient.Net.IO;
using RandomChat;
using System.Diagnostics;
using System.Net.Sockets;
using System.Windows;

namespace ChatClient.Net
{
    class Server
    {
        public void Connect(User user)
        {
            if (!IsConnected())
            {
                
                _socket = new TcpClient();
                _socket.Connect("127.0.0.1", 24901);
                _packetReader = new PacketReader(_socket.GetStream());

                _userData = user;
                SendInitDataPacket();
                
                Task.Run(() => ReadPackets());
            }
        }

        public bool IsConnected()
        {
            return _socket != null && _socket.Connected;
        }

        public void Disconnect()
        {
            if (IsConnected())
            {
                _socket.Client.Shutdown(SocketShutdown.Both);
                _socket.Client.Close();
                _socket.Close();
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
                        case OperationCode.StartChat:
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                                    mainWindow.ToChatPage();
                                });
                                
                                User connectedUser = _packetReader.ReadUser();
                                ChatStarted?.Invoke(connectedUser);
                                break;
                            }
                        case OperationCode.Message:                        
                            {
                                Message message = _packetReader.ReadMessage();
                                MessageReceived?.Invoke(message);
                                break;
                            }
                        case OperationCode.EndChat:
                            {
                                ChatEnded?.Invoke();
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

        private void SendPacket(PacketBuilder builder)
        {
            if (IsConnected())
            {
                _socket.Client.Send(builder.GetRawData());
            }
        }            

        private void SendInitDataPacket()
        {
            Debug.Assert(_userData != null);

            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteInitData(_userData);
            SendPacket(packetBuilder);
        }

        public event Action<Message>? MessageReceived;
        public event Action<User>? ChatStarted;
        public event Action? ChatEnded;

        private TcpClient? _socket;
        private PacketReader? _packetReader;
        private User? _userData;
    }
}
