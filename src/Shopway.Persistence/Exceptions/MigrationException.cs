using Shopway.Domain.Common.Errors;

namespace Shopway.Persistence.Exceptions;

[Serializable]
public sealed class MigrationException(Error error) : Exception(error.Message)
{
}
