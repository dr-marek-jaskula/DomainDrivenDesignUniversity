using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ValidationResult<FirstName> Create(string firstName)
    {
        var errors = Validate(firstName);

        if (errors.Any())
        {
            return ValidationResult<FirstName>.WithErrors(errors.ToArray());
        }

        return ValidationResult<FirstName>.WithoutErrors(new FirstName(firstName));
    }

    private static List<Error> Validate(string firstName)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errors.Add(FirstNameError.Empty);
        }

        if (firstName.Length > MaxLength)
        {
            errors.Add(FirstNameError.TooLong);
        }

        if (firstName.ContainsIllegalCharacter())
        {
            errors.Add(FirstNameError.ContainsIllegalCharacter);
        }

        if (firstName.ContainsDigit())
        {
            errors.Add(FirstNameError.ContainsDigit);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
