using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using System.Text;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    public const int BytesLong = 256;
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static Result<PasswordHash> Create(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<PasswordHash>(PasswordHashError.Empty);
        }

        var numberOfbytes = Encoding.ASCII.GetByteCount(passwordHash);

        if (numberOfbytes is not BytesLong)
        {
            return Result.Failure<PasswordHash>(PasswordHashError.BytesLong);
        }

        return new PasswordHash(passwordHash);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}