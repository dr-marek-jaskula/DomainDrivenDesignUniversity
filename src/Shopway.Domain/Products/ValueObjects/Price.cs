using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed record class Price : ValueObject<decimal>
{
    public const decimal MaxPrice = 100000;
    public const decimal MinPrice = 0;

    public static readonly Error TooLow = Error.New(
        $"{nameof(Price)}.{nameof(TooLow)}",
        $"{nameof(Price)} must be at least {MinPrice}.");

    public static readonly Error TooHigh = Error.New(
        $"{nameof(Price)}.{nameof(TooHigh)}",
        $"{nameof(Price)} must be at most {MaxPrice}.");

    private Price(decimal price) : base(price)
    {
    }

    public static ValidationResult<Price> Create(decimal price)
    {
        var errors = Validate(price);
        return errors.CreateValidationResult(() => new Price(decimal.Round(price, 2)));
    }

    public static IList<Error> Validate(decimal price)
    {
        return EmptyList<Error>()
            .If(price < MinPrice, TooLow)
            .If(price > MaxPrice, TooHigh);
    }
}

