using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

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