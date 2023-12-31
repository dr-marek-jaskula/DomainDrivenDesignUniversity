using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Orders.ValueObjects;

public sealed class Discount : ValueObject
{
    public const decimal MaxDiscount = 0.5m;
    public const decimal MinDiscount = 0;

    public new decimal Value { get; }

    private Discount(decimal value)
    {
        Value = value;
    }

    public static ValidationResult<Discount> Create(decimal price)
    {
        var errors = Validate(price);
        return errors.CreateValidationResult(() => new Discount(decimal.Round(price, 2)));
    }

    public static IList<Error> Validate(decimal price)
    {
        return EmptyList<Error>()
            .If(price < MinDiscount, DiscountError.TooLow)
            .If(price > MaxDiscount, DiscountError.TooHigh);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

