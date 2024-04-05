using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

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