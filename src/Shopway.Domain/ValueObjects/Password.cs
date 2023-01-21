using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using System.Text.RegularExpressions;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class Password : ValueObject
{
    private static readonly Regex _regex = new(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{0,}$", RegexOptions.Compiled);
    
    public const int MaxLength = 30;
    public const int MinLength = 9;
    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Failure<Password>(PasswordError.Empty);
        }

        if (password.Length < MinLength)
        {
            return Result.Failure<Password>(PasswordError.TooShort);
        }

        if (password.Length > MaxLength)
        {
            return Result.Failure<Password>(PasswordError.TooLong);
        }

        if (!_regex.IsMatch(password))
        {
            return Result.Failure<Password>(PasswordError.Invalid);
        }

        return new Password(password);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}