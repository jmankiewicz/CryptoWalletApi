﻿namespace CryptoWalletApi.Middlewares.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
        
    }
}
