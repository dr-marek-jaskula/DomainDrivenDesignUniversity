using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Products.Errors.DomainErrors;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class Price : ValueObject
{
    public const decimal MaxPrice = 100000;
    public const decimal MinPrice = 0;

    public new decimal Value { get; }

    private Price(decimal value)
    {
        Value = value;
    }

    public static ValidationResult<Price> Create(decimal price)
    {
        var errors = Validate(price);
        return errors.CreateValidationResult(() => new Price(decimal.Round(price, 2)));
    }

    public static IList<Error> Validate(decimal price)
    {
        return EmptyList<Error>()
            .If(price < MinPrice, PriceError.TooLow)
            .If(price > MaxPrice, PriceError.TooHigh);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

