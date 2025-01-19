﻿namespace ChatClient.Common
{
    internal enum ClientToServerOperations : byte
    {
        Invalid = 0,
        InitData = 1
    }

    internal enum ServerToClientOperations : byte
    {
        Invalid = 0,
        ConnectionEstablished = 1,
        ReceivingMessage = 2
    }
}
