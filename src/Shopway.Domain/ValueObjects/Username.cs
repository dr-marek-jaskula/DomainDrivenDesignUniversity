using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using Shopway.Domain.BaseTypes;

namespace Shopway.Domain.ValueObjects;

public sealed class Username : ValueObject
{
    public const int MaxLength = 30;

    private Username(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Username> Create(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Failure<Username>(UsernameError.Empty);
        }

        if (username.Length > MaxLength)
        {
            return Result.Failure<Username>(UsernameError.TooLong);
        }

        if (username.ContainsIllegalCharacter())
        {
            return Result.Failure<Username>(UsernameError.ContainsIllegalCharacter);
        }

        return new Username(username);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
