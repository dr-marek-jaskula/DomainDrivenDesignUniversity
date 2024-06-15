using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class Amount : ValueObject
{
    public const int MaxAmount = 1000;
    public const int MinAmount = 1;

    public static readonly Error TooLow = Error.New(
        $"{nameof(Amount)}.{nameof(TooLow)}",
        $"{nameof(Amount)} must be at least {MinAmount}.");

    public static readonly Error TooHigh = Error.New(
        $"{nameof(Amount)}.{nameof(TooHigh)}",
        $"{nameof(Amount)} must be at most {MaxAmount}.");

    private Amount(int value)
    {
        Value = value;
    }

    private Amount()
    {
    }

    public new int Value { get; }

    public static ValidationResult<Amount> Create(int amount)
    {
        var errors = Validate(amount);
        return errors.CreateValidationResult(() => new Amount(amount));
    }

    public static IList<Error> Validate(int amount)
    {
        return EmptyList<Error>()
            .If(amount < MinAmount, TooLow)
            .If(amount > MaxAmount, TooHigh);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
