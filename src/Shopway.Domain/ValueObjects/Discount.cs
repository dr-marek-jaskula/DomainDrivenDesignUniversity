using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class Discount : ValueObject
{
    public const decimal MaxDiscount = 0.5m;
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
            return Result.Failure<Discount>(DiscountError.TooLow);
        }

        if (price > MaxDiscount)
        {
            return Result.Failure<Discount>(DiscountError.TooHigh);
        }

        return new Discount(decimal.Round(price, 2));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

