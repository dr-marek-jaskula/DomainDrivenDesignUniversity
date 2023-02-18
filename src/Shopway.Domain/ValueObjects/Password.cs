using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

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

    public static ValidationResult<Password> Create(string password)
    {
        var errors = Validate(password);
        return errors.CreateValidationResult(() => new Password(password));
    }

    public static List<Error> Validate(string password)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add(PasswordError.Empty);
        }

        if (password.Length < MinLength)
        {
            errors.Add(PasswordError.TooShort);
        }

        if (password.Length > MaxLength)
        {
            errors.Add(PasswordError.TooLong);
        }

        if (!_regex.IsMatch(password))
        {
            errors.Add(PasswordError.Invalid);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}