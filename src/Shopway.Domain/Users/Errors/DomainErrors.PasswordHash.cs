using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users.Errors;

public static partial class DomainErrors
{
    public static class PasswordHashError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(PasswordHash)}.{nameof(Empty)}",
            $"{nameof(PasswordHash)} is empty.");

        public static readonly Error BytesLong = Error.New(
            $"{nameof(PasswordHash)}.{nameof(BytesLong)}",
            $"{nameof(PasswordHash)} needs to be less than {PasswordHash.BytesLong} bytes long.");
    }
}