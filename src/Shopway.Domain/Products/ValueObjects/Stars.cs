using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class Stars : ValueObject
{
    public static readonly decimal[] AdmissibleStars = [0, 0.5m, 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5];

    public static readonly Error Invalid = Error.New(
        $"{nameof(Stars)}.{nameof(Invalid)}",
        $"{nameof(Stars)} name must be: {AdmissibleStars.Join(',')}.");

    private Stars(decimal value)
    {
        Value = value;
    }

    private Stars()
    {

    }

    public new decimal Value { get; }

    public static ValidationResult<Stars> Create(decimal stars)
    {
        var errors = Validate(stars);
        return errors.CreateValidationResult(() => new Stars(stars));
    }

    public static IList<Error> Validate(decimal stars)
    {
        return EmptyList<Error>()
            .If(AdmissibleStars.NotContains(stars), Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

