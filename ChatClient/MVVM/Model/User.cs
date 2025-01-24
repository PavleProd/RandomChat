using System.ComponentModel;

namespace ChatClient.MVVM.Model
{
    class User : INotifyPropertyChanged
    {
        public User(string username)
        {
            Username = username;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Username { get; set; }
    }
}
