using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Stars : ValueObject
{
    public static readonly decimal[] AdmissibleStars = new decimal[11] { 0, 0.5m, 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5 };

    public decimal Value { get; }

    private Stars(decimal value)
    {
        Value = value;
    }

    public static ValidationResult<Stars> Create(decimal stars)
    {
        var errors = Validate(stars);

        if (errors.Any())
        {
            return ValidationResult<Stars>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Stars>.WithoutErrors(new Stars(stars));
    }

    public static List<Error> Validate(decimal stars)
    {
        var errors = Empty<Error>();

        if (!AdmissibleStars.Contains(stars))
        {
            errors.Add(StarsError.Invalid);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

