using System.ComponentModel;

namespace ChatClient.Common
{
    public enum MessageType
    {
        Incoming,
        Outgoing,
        Server
    };

    public class Message : INotifyPropertyChanged
    {
        public Message(string text, MessageType type, TimeOnly? time = null)
        {
            _text = text;
            SendingTime = time ?? TimeOnly.FromDateTime(DateTime.Now);
            Type = type;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TimeOnly SendingTime { get; init; }
        public MessageType? Type { get; init; }
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
