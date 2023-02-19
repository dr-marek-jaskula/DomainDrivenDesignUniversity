using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PriorityError
    {
        public static readonly Error TooHigh = new(
            $"{nameof(Priority)}.{nameof(TooHigh)}",
            $"{nameof(Priority)} must be at least {Priority.HighestPriority}");

        public static readonly Error TooLow = new(
            $"{nameof(Priority)}.{nameof(TooLow)}",
            $"{nameof(Priority)} must be at most {Priority.LowestPriority}");
    }
}