using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class TwoFactorTokenHash : ValueObject
{
    public const int NotHashedTokenFirstPartLength = 5;
    public const char NotHashedTokenSeparator = '-';
    public const int NotHashedTokenSecondPartLength = 5;

    public const int BytesLong = 514;

    public static readonly Error Empty = Error.New(
        $"{nameof(TwoFactorTokenHash)}.{nameof(Empty)}",
        $"{nameof(TwoFactorTokenHash)} is empty.");

    public static readonly Error InvalidBytesLong = Error.New(
        $"{nameof(TwoFactorTokenHash)}.{nameof(InvalidBytesLong)}",
        $"{nameof(TwoFactorTokenHash)} needs to be less than {BytesLong} bytes long.");

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
            .If(twoFactorTokenHash.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(Encoding.ASCII.GetByteCount(twoFactorTokenHash) > BytesLong, InvalidBytesLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
