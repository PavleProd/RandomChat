using ChatClient.Common;
using ChatClient.MVVM.Core;
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
            Messages = new ObservableCollection<string>();

            _server.MessageReceived += OnMessageReceived;
            ConnectToServerCommand = new RelayCommand(o => _server.Connect(Username), o => CanConnect());
            DisconnectFromServerCommand = new RelayCommand(o => _server.Disconnect(), o => _server.IsConnected());
        }

        public void OnMessageReceived(Message message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message.ToString()));
        }

        public bool CanConnect()
        {
            return !_server.IsConnected() && !string.IsNullOrEmpty(Username);
        }

        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand DisconnectFromServerCommand { get; set; }
        public string Username { get; set; }

        private Server _server;
    }
}
