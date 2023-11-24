using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class FirstNameError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(FirstName)}.{nameof(Empty)}",
            $"{nameof(FirstName)} is empty.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(FirstName)}.{nameof(TooLong)}",
            $"{nameof(FirstName)} must be at most {FirstName.MaxLength} characters long.");

        public static readonly Error ContainsIllegalCharacter = Error.New(
            $"{nameof(FirstName)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(FirstName)} contains illegal character.");

        public static readonly Error ContainsDigit = Error.New(
            $"{nameof(FirstName)}.{nameof(ContainsDigit)}",
            $"{nameof(FirstName)} contains digit.");
    }
}