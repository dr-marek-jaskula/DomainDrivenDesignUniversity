using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class Price : ValueObject
{
    public const decimal MaxPrice = 100000;
    public const decimal MinPrice = 0;

    public decimal Value { get; }

    private Price(decimal value)
    {
        Value = value;
    }

    public static Result<Price> Create(decimal price)
    {
        if (price < MinPrice)
        {
            return Result.Failure<Price>(PriceError.TooLow);
        }

        if (price > MaxPrice)
        {
            return Result.Failure<Price>(PriceError.TooHigh);
        }

        return new Price(decimal.Round(price, 2));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

