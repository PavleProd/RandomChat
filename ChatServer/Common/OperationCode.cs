namespace ChatServer.Common
{
    internal enum OperationCode : byte
    {
        None,
        InitData,
        LinkClients,
        Message,
        EndClientsLink,
        Invalid
    }
}
