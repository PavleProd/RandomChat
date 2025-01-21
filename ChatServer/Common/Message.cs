namespace ChatServer.Common
{
    readonly struct Message
    {
        public Message(string author, string text)
        {
            Author = author; // TODO: napravi bolje resenje umesto slanja imena svaki put
            Text = text;
            SendingTime = DateTime.Now; // TODO: sending time treba da se postavlja na now samo u porukama koje se salju
        }

        public override string ToString() 
        {
            return $"[{SendingTime}] | {Author}: {Text}";
        }

        public string Author { get; init; }
        public string Text { get; init; }
        public DateTime SendingTime { get; init; }
    }
}
