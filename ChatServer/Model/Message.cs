namespace ChatServer.Model
{
    readonly struct Message
    {
        public Message(string text, TimeOnly? time = null)
        {
            Text = text;
            SendingTime = time ?? TimeOnly.FromDateTime(DateTime.Now);
        }

        public TimeOnly SendingTime { get; init; }
        public string Text { get; init; }
    }
}
