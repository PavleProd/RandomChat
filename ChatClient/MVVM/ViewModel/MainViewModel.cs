using ChatClient.Common;
using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public MainViewModel()
        {
            _server = new Server();
            Messages = [];

            UserData = new User(String.Empty);
            TypedMessage = new Message(String.Empty);

            _server.MessageReceived += OnMessageReceived;
            ConnectToServerCommand = new RelayCommand(o => _server.Connect(UserData), o => !_server.IsConnected());
            DisconnectFromServerCommand = new RelayCommand(o => _server.Disconnect(), o => _server.IsConnected());
            SendMessageCommand = new RelayCommand(o => SendMessage(), o => !string.IsNullOrEmpty(TypedMessage.Text));
        }

        public void OnMessageReceived(Message message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        public bool CanConnect()
        {
            return !_server.IsConnected() && !string.IsNullOrEmpty(UserData.Username);
        }

        public void SendMessage()
        {            
            _server.SendMessage(TypedMessage);
            TypedMessage.Text = string.Empty;
        } 

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand DisconnectFromServerCommand { get; set; }
        public RelayCommand SendMessageCommand {  get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public Message TypedMessage { get; set; }
        public User UserData { get; set; }

        private readonly Server _server;
    }
}
