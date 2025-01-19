using ChatClient.MVVM.Core;
using ChatClient.Net;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public MainViewModel()
        {
            _server = new Server();
            ConnectToServerCommand = new RelayCommand(o => _server.Connect(Username), o => !string.IsNullOrEmpty(Username));
        }
        public RelayCommand ConnectToServerCommand { get; set; }
        public string Username { get; set; }

        private Server _server;
    }
}
