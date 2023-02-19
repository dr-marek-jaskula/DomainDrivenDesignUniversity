using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class EmailError
    {
        public static readonly Error Empty = new(
            $"{nameof(Email)}.{nameof(Empty)}",
            $"{nameof(Email)} is empty");

        public static readonly Error TooLong = new(
            $"{nameof(Email)}.{nameof(TooLong)}",
            $"{nameof(Email)} must be at most {Email.MaxLength} characters long");

        public static readonly Error Invalid = new(
            $"{nameof(Email)}.{nameof(Invalid)}",
            $"{nameof(Email)} must start from a letter, contain '@' and after that '.'");

        public static readonly Error EmailAlreadyTaken = new(
            $"{nameof(Email)}.{nameof(EmailAlreadyTaken)}",
            $"{nameof(Email)} is already taken");
    }
}