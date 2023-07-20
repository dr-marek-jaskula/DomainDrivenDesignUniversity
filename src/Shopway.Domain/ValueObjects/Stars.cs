using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Stars : ValueObject
{
    public static readonly decimal[] AdmissibleStars = new decimal[11] { 0, 0.5m, 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5 };

    public new decimal Value { get; }

    private Stars(decimal value)
    {
        Value = value;
    }

    public static ValidationResult<Stars> Create(decimal stars)
    {
        var errors = Validate(stars);
        return errors.CreateValidationResult(() => new Stars(stars));
    }

    public static IList<Error> Validate(decimal stars)
    {
        return EmptyList<Error>()
            .If(AdmissibleStars.NotContains(stars), StarsError.Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

