namespace Shopway.Application.Exceptions;

public sealed class UnavailableException : Exception
{
    public UnavailableException(string message) : base(message)
    {
    }
}