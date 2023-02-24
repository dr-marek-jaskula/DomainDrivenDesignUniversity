namespace Shopway.Application.Exceptions;

public sealed class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
