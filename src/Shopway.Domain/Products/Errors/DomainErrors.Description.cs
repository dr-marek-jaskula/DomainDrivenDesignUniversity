using Shopway.Domain.Errors;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Domain.Products.Errors;

public static partial class DomainErrors
{
    public static class DescriptionError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(Description)}.{nameof(Empty)}",
            $"{nameof(Description)} is empty.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(Description)}.{nameof(TooLong)}",
            $"{nameof(Description)} needs to be at most {Description.MaxLength} characters long.");
    }
}