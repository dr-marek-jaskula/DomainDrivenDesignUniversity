using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class Stars : ValueObject
{
    public static readonly decimal[] AdmissibleStars = new decimal[11] { 0, 0.5m, 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5 };

    public decimal Value { get; }

    private Stars(decimal value)
    {
        Value = value;
    }

    public static Result<Stars> Create(decimal stars)
    {
        if (!AdmissibleStars.Contains(stars))
        {
            return Result.Failure<Stars>(DomainErrors.StarsError.Invalid);
        }

        return new Stars(stars);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

