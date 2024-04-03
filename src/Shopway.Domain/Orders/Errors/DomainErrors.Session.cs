using Shopway.Domain.Errors;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Domain.Orders.Errors;

public static partial class DomainErrors
{
    public static class SessionError
    {
        public static readonly Error EmptyId = Error.New(
            $"{nameof(Session)}.{nameof(EmptyId)}",
            $"{nameof(EmptyId)} is empty.");

        public static readonly Error EmptySecret = Error.New(
            $"{nameof(Session)}.{nameof(EmptySecret)}",
            $"{nameof(EmptySecret)} is empty.");
    }
}