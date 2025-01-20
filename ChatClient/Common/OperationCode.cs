namespace ChatClient.Common
{
    internal enum OperationCode : byte
    {
        None = 0,
        InitData = 1,
        Message = 2,
        Invalid = 3
    }
}
