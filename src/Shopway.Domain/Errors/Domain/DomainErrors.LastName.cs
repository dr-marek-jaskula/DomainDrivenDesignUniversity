using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class LastNameError
    {
        public static readonly Error Empty = new(
            $"{nameof(LastName)}.{nameof(Empty)}",
            "LastName is empty");

        public static readonly Error TooLong = new(
            $"{nameof(LastName)}.{nameof(TooLong)}",
            $"{nameof(LastName)} must be at most {LastName.MaxLength} characters long");

        public static readonly Error ContainsIllegalCharacter = new(
            $"{nameof(LastName)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(LastName)} contains illegal character");

        public static readonly Error ContainsDigit = new(
            $"{nameof(LastName)}.{nameof(ContainsDigit)}",
            $"{nameof(LastName)} contains digit");
    }
}