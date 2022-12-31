using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

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
            return Result.Failure<LastName>(DomainErrors.LastNameError.Empty);
        }

        if (lastName.Length > MaxLength)
        {
            return Result.Failure<LastName>(DomainErrors.LastNameError.TooLong);
        }

        if (lastName.ContainsIllegalCharacter())
        {
            return Result.Failure<LastName>(DomainErrors.LastNameError.ContainsIllegalCharacter);
        }

        if (lastName.ContainsDigit())
        {
            return Result.Failure<LastName>(DomainErrors.LastNameError.ContainsDigit);
        }

        return new LastName(lastName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}