using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class RevisionError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(Revision)}.{nameof(Empty)}",
            $"{nameof(Revision)} is empty");

        public static readonly Error TooLong = Error.New(
            $"{nameof(Revision)}.{nameof(TooLong)}",
            $"{nameof(Revision)} must be at most {Revision.MaxLength} characters long");
    }
}