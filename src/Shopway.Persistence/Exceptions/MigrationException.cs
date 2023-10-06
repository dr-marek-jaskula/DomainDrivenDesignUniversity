namespace Shopway.Persistence.Exceptions;

[Serializable]
public sealed class MigrationException : Exception
{
    public MigrationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}