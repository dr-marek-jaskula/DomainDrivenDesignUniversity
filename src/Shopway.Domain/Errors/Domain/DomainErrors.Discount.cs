using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class DiscountError
    {
        public static readonly Error TooLow = Error.New(
            $"{nameof(Discount)}.{nameof(TooLow)}",
            $"{nameof(Discount)} must be at least {Discount.MinDiscount}");

        public static readonly Error TooHigh = Error.New(
            $"{nameof(Discount)}.{nameof(TooHigh)}",
            $"{nameof(Discount)} must be at most {Discount.MaxDiscount}");
    }
}