using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class AmountError
    {
        public static readonly Error TooLow = Error.New(
            $"{nameof(Amount)}.{nameof(TooLow)}",
            $"{nameof(Amount)} must be at least {Amount.MinAmount}");

        public static readonly Error TooHigh = Error.New(
            $"{nameof(Amount)}.{nameof(TooHigh)}",
            $"{nameof(Amount)} must be at most {Amount.MaxAmount}");
    }
}