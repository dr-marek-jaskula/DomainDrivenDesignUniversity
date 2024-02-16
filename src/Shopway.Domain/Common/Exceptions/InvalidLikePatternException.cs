namespace Shopway.Domain.Common.Exceptions;

public sealed class InvalidLikePatternException : Exception
{
    private const string _message = "Invalid like pattern: ";

    public InvalidLikePatternException(string searchPattern)
        : base($"{_message}{searchPattern}")
    {
    }

    public InvalidLikePatternException(string searchPattern, Exception innerException)
        : base($"{_message}{searchPattern}", innerException)
    {
    }
}