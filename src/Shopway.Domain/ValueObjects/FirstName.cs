using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
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
        return errors.CreateValidationResult(() => new FirstName(firstName));
    }

    public static IList<Error> Validate(string firstName)
    {
        return EmptyList<Error>()
            .If(firstName.IsNullOrEmptyOrWhiteSpace(), FirstNameError.Empty)
            .If(firstName.Length > MaxLength, FirstNameError.TooLong)
            .If(firstName.ContainsIllegalCharacter(), FirstNameError.ContainsIllegalCharacter)
            .If(firstName.ContainsDigit(), FirstNameError.ContainsDigit);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
