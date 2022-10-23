using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class Discount : ValueObject
{
    public const decimal MaxDiscount = 1;
    public const decimal MinDiscount = 0;

    public decimal Value { get; }

    private Discount(decimal value)
    {
        Value = value;
    }

    public static Result<Discount> Create(decimal price)
    {
        if (price < MinDiscount)
        {
            return Result.Failure<Discount>(DomainErrors.DiscountError.TooLow);
        }

        if (price > MaxDiscount)
        {
            return Result.Failure<Discount>(DomainErrors.DiscountError.TooHigh);
        }

        return new Discount(decimal.Round(price, 2));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

