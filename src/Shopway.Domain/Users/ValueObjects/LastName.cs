using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<LastName> Create(string lastName)
    {
        var errors = Validate(lastName);
        return errors.CreateValidationResult(() => new LastName(lastName));
    }

    public static IList<Error> Validate(string lastName)
    {
        return EmptyList<Error>()
            .If(lastName.IsNullOrEmptyOrWhiteSpace(), LastNameError.Empty)
            .If(lastName.Length > MaxLength, LastNameError.TooLong)
            .If(lastName.ContainsIllegalCharacter(), LastNameError.ContainsIllegalCharacter)
            .If(lastName.ContainsDigit(), LastNameError.ContainsDigit);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}