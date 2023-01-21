using System.Text;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using Shopway.Domain.BaseTypes;

namespace Shopway.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    public const int BytesLong = 514;
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

        if (numberOfbytes > BytesLong)
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