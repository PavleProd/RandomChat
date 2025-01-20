namespace ChatServer.Common
{
    readonly struct Message
    {
        public Message(string authorId, string text)
        {
            AuthorId = authorId;
            Text = text;
            SendingTime = DateTime.Now; // TODO: sending time treba da se postavlja na now samo u porukama koje se salju
        }

        public override string ToString() 
        {
            return $"[{SendingTime}] | {AuthorId}: {Text}";
        }

        public string AuthorId { get; init; }
        public string Text { get; init; }
        public DateTime SendingTime { get; init; }
    }
}
