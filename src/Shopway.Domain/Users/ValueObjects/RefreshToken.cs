using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
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

    public static IList<Error> Validate(string refreshToken)
    {
        return EmptyList<Error>()
            .If(refreshToken.IsNullOrEmptyOrWhiteSpace(), RefreshTokenError.Empty)
            .If(refreshToken.Length != Length, RefreshTokenError.InvalidLength);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
