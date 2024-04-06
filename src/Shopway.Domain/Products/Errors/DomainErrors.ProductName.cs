using Shopway.Domain.Common.Errors;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Domain.Products.Errors;

public static partial class DomainErrors
{
    public static class ProductNameError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(ProductName)}.{nameof(Empty)}",
            $"{nameof(ProductName)} is empty.");

        public static readonly Error TooLong = Error.New(
            $"{nameof(ProductName)}.{nameof(TooLong)}",
            $"{nameof(ProductName)} must be at most {ProductName.MaxLength} characters long.");

        public static readonly Error ContainsIllegalCharacter = Error.New(
            $"{nameof(ProductName)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(ProductName)} contains illegal character.");
    }
}