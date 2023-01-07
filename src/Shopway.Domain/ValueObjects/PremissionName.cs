using Shopway.Domain.Utilities;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class PremissionName : ValueObject
{
    public const int MaxLength = 50;

    private PremissionName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PremissionName> Create(string premissionName)
    {
        if (string.IsNullOrWhiteSpace(premissionName))
        {
            return Result.Failure<PremissionName>(PremissionNameError.Empty);
        }

        if (premissionName.Length > MaxLength)
        {
            return Result.Failure<PremissionName>(PremissionNameError.TooLong);
        }

        if (premissionName.ContainsIllegalCharacter())
        {
            return Result.Failure<PremissionName>(PremissionNameError.ContainsIllegalCharacter);
        }

        return new PremissionName(premissionName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
