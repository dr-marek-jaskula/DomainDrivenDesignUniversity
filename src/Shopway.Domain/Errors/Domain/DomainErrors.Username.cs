using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class UsernameError
    {
        public static readonly Error Empty = new(
            $"{nameof(Username)}.{nameof(Empty)}",
            $"{nameof(Username)} name is empty");

        public static readonly Error TooLong = new(
            $"{nameof(Username)}.{nameof(TooLong)}",
            $"{nameof(Username)} name must be at most {Username.MaxLength} characters");

        public static readonly Error ContainsIllegalCharacter = new(
            $"{nameof(Username)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(Username)} contains illegal character");
    }
}