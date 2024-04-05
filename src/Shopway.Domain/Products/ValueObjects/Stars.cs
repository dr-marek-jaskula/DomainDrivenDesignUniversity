using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Products.Errors.DomainErrors;

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

