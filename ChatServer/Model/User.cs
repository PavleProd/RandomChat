namespace ChatServer.Model
{
    readonly struct User
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; init; }
    }
}
