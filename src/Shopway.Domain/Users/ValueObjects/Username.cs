using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class Username : ValueObject
{
    public const int MaxLength = 30;

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
            .If(username.IsNullOrEmptyOrWhiteSpace(), UsernameError.Empty)
            .If(username.Length > MaxLength, UsernameError.TooLong)
            .If(username.ContainsIllegalCharacter(), UsernameError.ContainsIllegalCharacter);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
