﻿namespace Shopway.Persistence.Exceptions;

[Serializable]
public sealed class ConfigurationException : Exception
{
    public ConfigurationException(string message)
        : base(message)
    {
    }
}
