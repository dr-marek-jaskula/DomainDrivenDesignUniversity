using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    public static readonly Error Empty = Error.New(
        $"{nameof(FirstName)}.{nameof(Empty)}",
        $"{nameof(FirstName)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(FirstName)}.{nameof(TooLong)}",
        $"{nameof(FirstName)} must be at most {MaxLength} characters long.");

    public static readonly Error ContainsIllegalCharacter = Error.New(
        $"{nameof(FirstName)}.{nameof(ContainsIllegalCharacter)}",
        $"{nameof(FirstName)} contains illegal character.");

    public static readonly Error ContainsDigit = Error.New(
        $"{nameof(FirstName)}.{nameof(ContainsDigit)}",
        $"{nameof(FirstName)} contains digit.");

    private FirstName(string value)
    {
        Value = value;
    }

    private FirstName()
    {
    }

    public new string Value { get; }

    public static ValidationResult<FirstName> Create(string firstName)
    {
        var errors = Validate(firstName);
        return errors.CreateValidationResult(() => new FirstName(firstName));
    }

    public static IList<Error> Validate(string firstName)
    {
        return EmptyList<Error>()
            .If(firstName.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(firstName.Length > MaxLength, TooLong)
            .If(firstName.ContainsIllegalCharacter(), ContainsIllegalCharacter)
            .If(firstName.ContainsDigit(), ContainsDigit);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
