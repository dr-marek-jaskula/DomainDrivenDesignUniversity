using Shopway.Domain.Errors;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users.Errors;

public static partial class DomainErrors
{
    public static class TwoFactorTokenError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(TwoFactorTokenHash)}.{nameof(Empty)}",
            $"{nameof(TwoFactorTokenHash)} is empty.");

        public static readonly Error BytesLong = Error.New(
            $"{nameof(PasswordHash)}.{nameof(BytesLong)}",
            $"{nameof(PasswordHash)} needs to be less than {PasswordHash.BytesLong} bytes long.");
    }
}