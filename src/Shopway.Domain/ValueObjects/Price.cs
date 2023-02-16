using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

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

    public static ValidationResult<Price> Create(decimal price)
    {
        var errors = Validate(price);

        if (errors.Any())
        {
            return ValidationResult<Price>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Price>.WithoutErrors(new Price(decimal.Round(price, 2)));
    }

    private static List<Error> Validate(decimal price)
    {
        var errors = Empty<Error>();

        if (price < MinPrice)
        {
            errors.Add(PriceError.TooLow);
        }

        if (price > MaxPrice)
        {
            errors.Add(PriceError.TooHigh);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

