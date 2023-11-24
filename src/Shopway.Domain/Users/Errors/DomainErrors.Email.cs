using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class EmailError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(Email)}.{nameof(Empty)}",
            $"{nameof(Email)} is empty.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(Email)}.{nameof(TooLong)}",
            $"{nameof(Email)} must be at most {Email.MaxLength} characters long.");

        public static readonly Error Invalid = Error.New(
            $"{nameof(Email)}.{nameof(Invalid)}",
            $"{nameof(Email)} must start from a letter, contain '@' and after that '.'.");

        public static readonly Error EmailAlreadyTaken = Error.New(
            $"{nameof(Email)}.{nameof(EmailAlreadyTaken)}",
            $"{nameof(Email)} is already taken.");
    }
}