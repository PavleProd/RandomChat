using ChatClient.Common;
using ChatClient.MVVM.Core;
using ChatClient.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            _server = new Server();
            Messages = new ObservableCollection<string>();

            _server.MessageReceived += OnMessageReceived;
            ConnectToServerCommand = new RelayCommand(o => _server.Connect(Username), o => !_server.IsConnected());
            DisconnectFromServerCommand = new RelayCommand(o => _server.Disconnect(), o => _server.IsConnected());
            SendMessageCommand = new RelayCommand(o => SendMessage(), o => !string.IsNullOrEmpty(Message));
        }

        public void OnMessageReceived(Message message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message.ToString()));
        }

        public bool CanConnect()
        {
            return !_server.IsConnected() && !string.IsNullOrEmpty(Username);
        }

        public void SendMessage()
        {            
            _server.SendMessage(Message);
            Message = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand DisconnectFromServerCommand { get; set; }
        public RelayCommand SendMessageCommand {  get; set; }
        public string Username { get; set; }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private Server _server;
        private string _message;
    }
}
