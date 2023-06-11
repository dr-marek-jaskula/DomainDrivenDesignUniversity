using System.Text;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    public const int BytesLong = 514;
    public string Value { get; }

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