using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Orders.Errors.DomainErrors;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class Amount : ValueObject
{
    public const int MaxAmount = 1000;
    public const int MinAmount = 1;

    public new int Value { get; }

    private Amount(int value)
    {
        Value = value;
    }

    public static ValidationResult<Amount> Create(int amount)
    {
        var errors = Validate(amount);
        return errors.CreateValidationResult(() => new Amount(amount));
    }

    public static IList<Error> Validate(int amount)
    {
        return EmptyList<Error>()
            .If(amount < MinAmount, AmountError.TooLow)
            .If(amount > MaxAmount, AmountError.TooHigh);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}