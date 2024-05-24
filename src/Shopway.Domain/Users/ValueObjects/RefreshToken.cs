using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class RefreshToken : ValueObject
{
    public const int Length = 32;

    public static readonly Error Empty = Error.New(
        $"{nameof(RefreshToken)}.{nameof(Empty)}",
        $"{nameof(RefreshToken)} is empty.");

    public static readonly Error InvalidLength = Error.New(
        $"{nameof(RefreshToken)}.{nameof(InvalidLength)}",
        $"{nameof(RefreshToken)} length must be {Length} characters.");

    private RefreshToken(string value)
    {
        Value = value;
    }

    private RefreshToken()
    {
        
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
            .If(refreshToken.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(refreshToken.Length != Length, InvalidLength);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
