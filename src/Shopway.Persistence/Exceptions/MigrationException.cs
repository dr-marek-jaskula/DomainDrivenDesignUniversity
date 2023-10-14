using Shopway.Domain.Errors;

namespace Shopway.Persistence.Exceptions;

[Serializable]
public sealed class MigrationException : Exception
{
    public MigrationException(Error error)
        : base(error.Message)
    {
    }
}