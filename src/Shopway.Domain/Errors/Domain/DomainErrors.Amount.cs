using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class AmountError
    {
        public static readonly Error TooLow = new(
            $"{nameof(Amount)}.{nameof(TooLow)}",
            $"{nameof(Amount)} must be at least {Amount.MinAmount}");

        public static readonly Error TooHigh = new(
            $"{nameof(Amount)}.{nameof(TooHigh)}",
            $"{nameof(Amount)} must be at most {Amount.MaxAmount}");
    }
}