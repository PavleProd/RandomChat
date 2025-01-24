using System.ComponentModel;

namespace ChatClient.MVVM.Model
{
    public class User : INotifyPropertyChanged
    {
        public User(string username)
        {
            _username = username;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Username
        { 
            get => _username; 
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _username;
    }
}
