using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PasswordError
    {
        public static readonly Error Empty = new(
            $"{nameof(Password)}.{nameof(Empty)}",
            $"{nameof(Password)} is empty");

        public static readonly Error TooShort = new(
            $"{nameof(Password)}.{nameof(TooShort)}",
            $"{nameof(Password)} needs to be at least {Password.MinLength} characters long");

        public static readonly Error TooLong = new(
            $"{nameof(Password)}.{nameof(TooLong)}",
            $"{nameof(Password)} needs to be at most {Password.MaxLength} characters long");

        public static readonly Error Invalid = new(
            $"{nameof(Password)}.{nameof(Invalid)}",
            $"{nameof(Password)} needs to contain at least one digit, one small letter and one capital letter");
    }
}