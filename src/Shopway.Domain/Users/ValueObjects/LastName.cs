using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    public static readonly Error Empty = Error.New(
        $"{nameof(LastName)}.{nameof(Empty)}",
        $"{nameof(LastName)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(LastName)}.{nameof(TooLong)}",
        $"{nameof(LastName)} must be at most {MaxLength} characters long.");

    public static readonly Error ContainsIllegalCharacter = Error.New(
        $"{nameof(LastName)}.{nameof(ContainsIllegalCharacter)}",
        $"{nameof(LastName)} contains illegal character.");

    public static readonly Error ContainsDigit = Error.New(
        $"{nameof(LastName)}.{nameof(ContainsDigit)}",
        $"{nameof(LastName)} contains digit.");

    private LastName(string value)
    {
        Value = value;
    }

    private LastName()
    {
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
            .If(lastName.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(lastName.Length > MaxLength, TooLong)
            .If(lastName.ContainsIllegalCharacter(), ContainsIllegalCharacter)
            .If(lastName.ContainsDigit(), ContainsDigit);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
