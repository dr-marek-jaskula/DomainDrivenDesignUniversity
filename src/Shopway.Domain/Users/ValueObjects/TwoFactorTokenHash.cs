using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using System.Text;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class TwoFactorTokenHash : ValueObject
{
    public const int NotHashedTokenFirstPartLength = 5;
    public const char NotHashedTokenSeparator = '-';
    public const int NotHashedTokenSecondPartLength = 5;

    public const int BytesLong = 514;

    private TwoFactorTokenHash(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<TwoFactorTokenHash> Create(string twoFactorTokenHash)
    {
        var errors = Validate(twoFactorTokenHash);
        return errors.CreateValidationResult(() => new TwoFactorTokenHash(twoFactorTokenHash));
    }

    public static IList<Error> Validate(string twoFactorTokenHash)
    {
        return EmptyList<Error>()
            .If(twoFactorTokenHash.IsNullOrEmptyOrWhiteSpace(), TwoFactorTokenError.Empty)
            .If(Encoding.ASCII.GetByteCount(twoFactorTokenHash) > BytesLong, PasswordHashError.BytesLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}