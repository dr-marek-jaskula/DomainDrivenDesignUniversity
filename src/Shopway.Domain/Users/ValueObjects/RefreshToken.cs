using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class RefreshToken : ValueObject
{
    public const int Length = 32;

    private RefreshToken(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<RefreshToken> Create(string refreshToken)
    {
        var errors = Validate(refreshToken);
        return errors.CreateValidationResult(() => new RefreshToken(refreshToken));
    }

    public static IList<Error> Validate(string username)
    {
        return EmptyList<Error>()
            .If(username.IsNullOrEmptyOrWhiteSpace(), RefreshTokenError.Empty)
            .If(username.Length != Length, RefreshTokenError.InvalidLength);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
