using Shopway.Domain.Errors;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users.Errors;

public static partial class DomainErrors
{
    public static class PasswordError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(Password)}.{nameof(Empty)}",
            $"{nameof(Password)} is empty.");

        public static readonly Error TooShort = Error.New(
            $"{nameof(Password)}.{nameof(TooShort)}",
            $"{nameof(Password)} needs to be at least {Password.MinLength} characters long.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(Password)}.{nameof(TooLong)}",
            $"{nameof(Password)} needs to be at most {Password.MaxLength} characters long.");

        public static readonly Error Invalid = Error.New(
            $"{nameof(Password)}.{nameof(Invalid)}",
            $"{nameof(Password)} needs to contain at least one digit, one small letter, one capital letter and one special character.");
    }
}