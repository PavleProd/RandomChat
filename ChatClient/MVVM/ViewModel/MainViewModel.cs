using ChatClient.Common;
using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using RandomChat;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            _server = new Server();
            Messages = [];

            ConnectedUser = new User(String.Empty);
            UserData = new User(String.Empty);
            TypedMessage = new Message(String.Empty, MessageType.Outgoing);

            SubscribeToEvents();
            InitCommands();
        }

        private void SubscribeToEvents()
        {
            _server.MessageReceived += OnMessageReceived;
            _server.ChatStarted += OnChatStarted;
            _server.ChatEnded += OnChatEnded;
        }

        public void OnMessageReceived(Message message)
        {
            AddMessage(message);
        }

        public void OnChatStarted(User connectedUser)
        {
            ConnectedUser.Username = connectedUser.Username;
            AddServerMessage($"User {ConnectedUser.Username} has joined the chat!");
        }

        public void OnChatEnded()
        {
            AddServerMessage($"User {ConnectedUser.Username} has left the chat!");
        }

        private void AddServerMessage(string text)
        {
            Message serverMessage = new(text, MessageType.Server);
            AddMessage(serverMessage);
        }

        private void InitCommands()
        {
            ConnectToServerCommand = new RelayCommand(o => _server.Connect(UserData), o => CanConnect());
            DisconnectFromServerCommand = new RelayCommand(o => _server.Disconnect(), o => _server.IsConnected());
            SendMessageCommand = new RelayCommand(o => SendMessage(), o => !string.IsNullOrEmpty(TypedMessage.Text));
        }
        private bool CanConnect()
        {
            return !_server.IsConnected() && !string.IsNullOrEmpty(UserData.Username);
        }

        private void SendMessage()
        {
            Message message = new Message(TypedMessage.Text, MessageType.Outgoing);
            _server.SendMessage(message);
            AddMessage(message);

            TypedMessage.Text = string.Empty;
        }

        private void AddMessage(Message message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(message);
            });
        }

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand DisconnectFromServerCommand { get; set; }
        public RelayCommand SendMessageCommand {  get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public Message TypedMessage { get; set; }
        public User UserData { get; set; }
        public User ConnectedUser { get; set; }

        private readonly Server _server;
    }
}
