using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Amount : ValueObject
{
    public const int MaxAmount = 1000;
    public const int MinAmount = 1;

    public int Value { get; }

    private Amount(int value)
    {
        Value = value;
    }

    public static ValidationResult<Amount> Create(int amount)
    {
        var errors = Validate(amount);

        if (errors.Any())
        {
            return ValidationResult<Amount>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Amount>.WithoutErrors(new Amount(amount));
    }

    public static List<Error> Validate(int amount)
    {
        var errors = Empty<Error>();

        if (amount < MinAmount)
        {
            errors.Add(AmountError.TooLow);
        }

        if (amount > MaxAmount)
        {
            errors.Add(AmountError.TooHigh);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}