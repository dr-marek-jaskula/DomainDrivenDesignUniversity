using Shopway.Domain.Errors;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Domain.Products.Errors;

public static partial class DomainErrors
{
    public static class TitleError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(Title)}.{nameof(Empty)}",
            $"{nameof(Title)} is empty.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(Title)}.{nameof(TooLong)}",
            $"{nameof(Title)} needs to be at most {Title.MaxLength} characters long.");
    }
}