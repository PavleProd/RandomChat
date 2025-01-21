namespace ChatServer
{
    internal class Program
    {
        static Program()
        {
            RandomChat = new Server();
        }

        public static void Main(string[] args)
        {
            RandomChat.Process();
        }

        public static Server RandomChat { get; set; }
    }
}
