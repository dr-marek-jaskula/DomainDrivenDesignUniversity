using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class Discount : ValueObject
{
    public const decimal MaxDiscount = 0.5m;
    public const decimal MinDiscount = 0;

    public static readonly Error TooLow = Error.New(
        $"{nameof(Discount)}.{nameof(TooLow)}",
        $"{nameof(Discount)} must be at least {MinDiscount}.");

    public static readonly Error TooHigh = Error.New(
        $"{nameof(Discount)}.{nameof(TooHigh)}",
        $"{nameof(Discount)} must be at most {MaxDiscount}.");

    private Discount(decimal value)
    {
        Value = value;
    }

    private Discount()
    {
    }

    public new decimal Value { get; }

    public static ValidationResult<Discount> Create(decimal discount)
    {
        var errors = Validate(discount);
        return errors.CreateValidationResult(() => new Discount(decimal.Round(discount, 2)));
    }

    public static IList<Error> Validate(decimal discount)
    {
        return EmptyList<Error>()
            .If(discount < MinDiscount, TooLow)
            .If(discount > MaxDiscount, TooHigh);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

