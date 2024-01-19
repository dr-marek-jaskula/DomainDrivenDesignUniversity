using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using System.Text;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    public const int BytesLong = 514;
    public new string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static ValidationResult<PasswordHash> Create(string passwordHash)
    {
        var errors = Validate(passwordHash);
        return errors.CreateValidationResult(() => new PasswordHash(passwordHash));
    }

    public static IList<Error> Validate(string passwordHash)
    {
        return EmptyList<Error>()
            .If(passwordHash.IsNullOrEmptyOrWhiteSpace(), PasswordHashError.Empty)
            .If(Encoding.ASCII.GetByteCount(passwordHash) > BytesLong, PasswordHashError.BytesLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}