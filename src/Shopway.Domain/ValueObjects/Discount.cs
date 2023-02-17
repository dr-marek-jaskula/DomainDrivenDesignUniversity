using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

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

    public static ValidationResult<Discount> Create(decimal price)
    {
        var errors = Validate(price);

        if (errors.Any())
        {
            return ValidationResult<Discount>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Discount>.WithoutErrors(new Discount(decimal.Round(price, 2)));
    }

    public static List<Error> Validate(decimal price)
    {
        var errors = Empty<Error>();

        if (price < MinDiscount)
        {
            errors.Add(DiscountError.TooLow);
        }

        if (price > MaxDiscount)
        {
            errors.Add(DiscountError.TooHigh);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

