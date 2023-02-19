using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class ProductNameError
    {
        public static readonly Error Empty = new(
            $"{nameof(ProductName)}.{nameof(Empty)}",
            $"{nameof(ProductName)} is empty");

        public static readonly Error TooLong = new(
            $"{nameof(ProductName)}.{nameof(TooLong)}",
            $"{nameof(ProductName)} must be at most {ProductName.MaxLength} characters long");

        public static readonly Error ContainsIllegalCharacter = new(
            $"{nameof(ProductName)}.{nameof(ContainsIllegalCharacter)}",
            $"{nameof(ProductName)} contains illegal character");
    }
}