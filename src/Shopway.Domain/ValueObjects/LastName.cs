using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ValidationResult<LastName> Create(string lastName)
    {
        var errors = Validate(lastName);
        return errors.CreateValidationResult(() => new LastName(lastName));
    }

    public static List<Error> Validate(string lastName)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add(LastNameError.Empty);
        }

        if (lastName.Length > MaxLength)
        {
            errors.Add(LastNameError.TooLong);
        }

        if (lastName.ContainsIllegalCharacter())
        {
            errors.Add(LastNameError.ContainsIllegalCharacter);
        }

        if (lastName.ContainsDigit())
        {
            errors.Add(LastNameError.ContainsDigit);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}