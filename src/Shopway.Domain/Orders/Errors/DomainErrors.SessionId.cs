using Shopway.Domain.Errors;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Domain.Orders.Errors;

public static partial class DomainErrors
{
    public static class SessionIdError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(SessionId)}.{nameof(Empty)}",
            $"{nameof(SessionId)} is empty.");
    }
}