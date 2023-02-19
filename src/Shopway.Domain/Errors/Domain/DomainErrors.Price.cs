using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PriceError
    {
        public static readonly Error TooLow = new(
            $"{nameof(Price)}.{nameof(TooLow)}",
            $"{nameof(Price)} must be at least {Price.MinPrice}");

        public static readonly Error TooHigh = new(
            $"{nameof(Price)}.{nameof(TooHigh)}",
            $"{nameof(Price)} must be at most {Price.MaxPrice}");
    }
}