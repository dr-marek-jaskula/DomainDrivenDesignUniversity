using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using System.Text.RegularExpressions;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public const int MaxLength = 40;

    private static readonly Regex _regex = new(@"^([a-zA-Z])([a-zA-Z0-9]+)\.?([a-zA-Z0-9]+)@([a-z]+)\.[a-z]{2,3}$", RegexOptions.Compiled);
    
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<Email>(EmailError.Empty);
        }

        if (email.Length > MaxLength)
        {
            return Result.Failure<Email>(EmailError.Empty);
        }

        if (!_regex.IsMatch(email))
        {
            return Result.Failure<Email>(EmailError.Invalid);
        }

        return new Email(email);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
