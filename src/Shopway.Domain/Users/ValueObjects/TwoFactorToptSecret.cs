using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed record class TwoFactorToptSecret : ValueObject<string>
{
    public const int BytesLong = 32;

    public static readonly Error Empty = Error.New(
        $"{nameof(TwoFactorToptSecret)}.{nameof(Empty)}",
        $"{nameof(TwoFactorToptSecret)} is empty.");

    public static readonly Error InvalidBytesLong = Error.New(
        $"{nameof(TwoFactorToptSecret)}.{nameof(InvalidBytesLong)}",
        $"{nameof(TwoFactorToptSecret)} needs to be {BytesLong} bytes long.");

    private TwoFactorToptSecret(string twoFactorToptSecret) : base(twoFactorToptSecret)
    {
    }

    public static ValidationResult<TwoFactorToptSecret> Create(string twoFactorToptSecret)
    {
        var errors = Validate(twoFactorToptSecret);
        return errors.CreateValidationResult(() => new TwoFactorToptSecret(twoFactorToptSecret));
    }

    public static IList<Error> Validate(string twoFactorTokenHash)
    {
        return EmptyList<Error>()
            .If(twoFactorTokenHash.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(Encoding.ASCII.GetByteCount(twoFactorTokenHash) != BytesLong, InvalidBytesLong);
    }
}
