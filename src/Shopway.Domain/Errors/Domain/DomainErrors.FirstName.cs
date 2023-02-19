using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class FirstNameError
    {
        public static readonly Error Empty = new(
            $"{nameof(FirstName)}.{nameof(Empty)}",
            $"{nameof(FirstName)} is empty");

        public static readonly Error TooLong = new(
            $"{nameof(FirstName)}.{nameof(TooLong)}",
            $"{nameof(FirstName)} must be at most {FirstName.MaxLength} characters long");

        public static readonly Error ContainsIllegalCharacter = new(
            $"{nameof(FirstName)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(FirstName)} contains illegal character");

        public static readonly Error ContainsDigit = new(
            $"{nameof(FirstName)}.{nameof(ContainsDigit)}",
            $"{nameof(FirstName)} contains digit");
    }
}