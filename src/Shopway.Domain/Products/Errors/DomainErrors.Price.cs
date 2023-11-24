using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PriceError
    {
        public static readonly Error TooLow = Error.New(
            $"{nameof(Price)}.{nameof(TooLow)}",
            $"{nameof(Price)} must be at least {Price.MinPrice}.");

        public static readonly Error TooHigh = Error.New(
            $"{nameof(Price)}.{nameof(TooHigh)}",
            $"{nameof(Price)} must be at most {Price.MaxPrice}.");
    }
}