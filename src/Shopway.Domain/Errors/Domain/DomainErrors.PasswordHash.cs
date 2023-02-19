using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PasswordHashError
    {
        public static readonly Error Empty = new(
            $"{nameof(PasswordHash)}.{nameof(Empty)}",
            $"{nameof(PasswordHash)} is empty");

        public static readonly Error BytesLong = new(
            $"{nameof(PasswordHash)}.{nameof(BytesLong)}",
            $"{nameof(PasswordHash)} needs to be less than {PasswordHash.BytesLong} bytes long");
    }
}