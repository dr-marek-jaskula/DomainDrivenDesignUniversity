using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class Username : ValueObject
{
    public const int MaxLength = 30;

    public static readonly Error Empty = Error.New(
        $"{nameof(Username)}.{nameof(Empty)}",
        $"{nameof(Username)} name is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(Username)}.{nameof(TooLong)}",
        $"{nameof(Username)} name must be at most {MaxLength} characters.");

    public static readonly Error ContainsIllegalCharacter = Error.New(
        $"{nameof(Username)}.{nameof(ContainsIllegalCharacter)}",
        $"{nameof(Username)} contains illegal character.");

    private Username(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<Username> Create(string username)
    {
        var errors = Validate(username);
        return errors.CreateValidationResult(() => new Username(username));
    }

    public static IList<Error> Validate(string username)
    {
        return EmptyList<Error>()
            .If(username.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(username.Length > MaxLength, TooLong)
            .If(username.ContainsIllegalCharacter(), ContainsIllegalCharacter);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
