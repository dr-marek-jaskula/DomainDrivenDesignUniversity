namespace Shopway.Presentation.Exceptions;

public class ForbidException : Exception
{
    public ForbidException(string message) : base(message)
    {
    }
}