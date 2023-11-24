using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class Stars : ValueObject
{
    public static readonly decimal[] AdmissibleStars = [0, 0.5m, 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5];

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

