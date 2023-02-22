namespace Shopway.Presentation.Exceptions;

public sealed class ForbidException : Exception
{
    public ForbidException(string message) : base(message)
    {
    }
}