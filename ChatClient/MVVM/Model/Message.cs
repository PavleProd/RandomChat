using System.ComponentModel;

namespace ChatClient.Common
{
    class Message : INotifyPropertyChanged
    {
        public Message(string text, TimeOnly? time = null)
        {
            _text = text;
            SendingTime = time ?? TimeOnly.FromDateTime(DateTime.Now);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TimeOnly SendingTime { get; init; }
        public string Text 
        { 
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private string _text;
    }
}
