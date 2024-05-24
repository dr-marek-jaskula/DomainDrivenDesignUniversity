using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class AccessToken : ValueObject
{
    public const int Length = 384;

    public static readonly Error Empty = Error.New(
        $"{nameof(AccessToken)}.{nameof(Empty)}",
        $"{nameof(AccessToken)} is empty.");

    public static readonly Error InvalidLength = Error.New(
        $"{nameof(AccessToken)}.{nameof(InvalidLength)}",
        $"{nameof(AccessToken)} length must be {Length} characters.");

    private AccessToken(string value)
    {
        Value = value;
    }

    private AccessToken()
    {

    }

    public new string Value { get; }

    public static ValidationResult<AccessToken> Create(string accessToken)
    {
        var errors = Validate(accessToken);
        return errors.CreateValidationResult(() => new AccessToken(accessToken));
    }

    public static IList<Error> Validate(string accessToken)
    {
        return EmptyList<Error>()
            .If(accessToken.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(accessToken.Length != Length, InvalidLength);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
