using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PasswordHashError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(PasswordHash)}.{nameof(Empty)}",
            $"{nameof(PasswordHash)} is empty");

        public static readonly Error BytesLong = Error.New(
            $"{nameof(PasswordHash)}.{nameof(BytesLong)}",
            $"{nameof(PasswordHash)} needs to be less than {PasswordHash.BytesLong} bytes long");
    }
}