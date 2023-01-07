using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<LastName> Create(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<LastName>(LastNameError.Empty);
        }

        if (lastName.Length > MaxLength)
        {
            return Result.Failure<LastName>(LastNameError.TooLong);
        }

        if (lastName.ContainsIllegalCharacter())
        {
            return Result.Failure<LastName>(LastNameError.ContainsIllegalCharacter);
        }

        if (lastName.ContainsDigit())
        {
            return Result.Failure<LastName>(LastNameError.ContainsDigit);
        }

        return new LastName(lastName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}