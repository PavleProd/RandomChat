﻿namespace ChatClient.Common
{
    internal enum OperationCode : byte
    {
        None,
        InitData,
        StartChat,
        Message,
        EndChat,
        Invalid
    }
}
