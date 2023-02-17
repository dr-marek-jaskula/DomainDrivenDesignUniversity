using System.Text;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using static Shopway.Domain.Errors.DomainErrors;
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

        if (errors.Any())
        {
            return ValidationResult<PasswordHash>.WithErrors(errors.ToArray());
        }

        return ValidationResult<PasswordHash>.WithoutErrors(new PasswordHash(passwordHash));
    }

    public static List<Error> Validate(string passwordHash)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            errors.Add(PasswordHashError.Empty);
        }

        var numberOfbytes = Encoding.ASCII.GetByteCount(passwordHash);

        if (numberOfbytes > BytesLong)
        {
            errors.Add(PasswordHashError.BytesLong);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}