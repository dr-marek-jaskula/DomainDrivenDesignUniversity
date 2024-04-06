using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users.Errors;

public static partial class DomainErrors
{
    public static class LastNameError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(LastName)}.{nameof(Empty)}",
            $"{nameof(LastName)} is empty.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(LastName)}.{nameof(TooLong)}",
            $"{nameof(LastName)} must be at most {LastName.MaxLength} characters long.");

        public static readonly Error ContainsIllegalCharacter = Error.New(
            $"{nameof(LastName)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(LastName)} contains illegal character.");

        public static readonly Error ContainsDigit = Error.New(
            $"{nameof(LastName)}.{nameof(ContainsDigit)}",
            $"{nameof(LastName)} contains digit.");
    }
}