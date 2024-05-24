using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    public const int BytesLong = 514;

    public static readonly Error Empty = Error.New(
        $"{nameof(PasswordHash)}.{nameof(Empty)}",
        $"{nameof(PasswordHash)} is empty.");

    public static readonly Error InvalidBytesLong = Error.New(
        $"{nameof(PasswordHash)}.{nameof(InvalidBytesLong)}",
        $"{nameof(PasswordHash)} needs to be less than {BytesLong} bytes long.");

    private PasswordHash(string value)
    {
        Value = value;
    }

    private PasswordHash()
    {
    }

    public new string Value { get; }

    public static ValidationResult<PasswordHash> Create(string passwordHash)
    {
        var errors = Validate(passwordHash);
        return errors.CreateValidationResult(() => new PasswordHash(passwordHash));
    }

    public static IList<Error> Validate(string passwordHash)
    {
        return EmptyList<Error>()
            .If(passwordHash.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(Encoding.ASCII.GetByteCount(passwordHash) > BytesLong, InvalidBytesLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
