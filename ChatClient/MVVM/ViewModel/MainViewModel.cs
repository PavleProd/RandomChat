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

            UserData = new User(String.Empty);
            TypedMessage = new Message(String.Empty);

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
            ConnectedUser = connectedUser;
            AddMessage(new Message($"User {ConnectedUser.Username} has joined the chat!"));
        }

        public void OnChatEnded()
        {
            AddMessage(new Message($"User {ConnectedUser.Username} has left the chat!"));
            ConnectedUser = null;
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
            _server.SendMessage(TypedMessage);
            TypedMessage.Text = string.Empty;
        }

        private void AddMessage(Message message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand DisconnectFromServerCommand { get; set; }
        public RelayCommand SendMessageCommand {  get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public Message TypedMessage { get; set; }
        public User UserData { get; set; }
        public User? ConnectedUser { get; set; }

        private readonly Server _server;
    }
}
