using Shopway.Domain.Common.Errors;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Domain.Orders.Errors;

public static partial class DomainErrors
{
    public static class AmountError
    {
        public static readonly Error TooLow = Error.New(
            $"{nameof(Amount)}.{nameof(TooLow)}",
            $"{nameof(Amount)} must be at least {Amount.MinAmount}.");

        public static readonly Error TooHigh = Error.New(
            $"{nameof(Amount)}.{nameof(TooHigh)}",
            $"{nameof(Amount)} must be at most {Amount.MaxAmount}.");
    }
}