using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FirstName>(DomainErrors.FirstNameError.Empty);
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<FirstName>(DomainErrors.FirstNameError.TooLong);
        }

        if (firstName.ContainsIllegalCharacter())
        {
            return Result.Failure<FirstName>(DomainErrors.FirstNameError.ContainsIllegalCharacter);
        }

        if (firstName.ContainsDigit())
        {
            return Result.Failure<FirstName>(DomainErrors.FirstNameError.ContainsDigit);
        }

        return new FirstName(firstName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
