using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

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

    public static ValidationResult<Email> Create(string email)
    {
        var errors = Validate(email);
        return errors.CreateValidationResult(() => new Email(email));
    }

    public static List<Error> Validate(string email)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrEmpty(email))
        {
            errors.Add(EmailError.Empty);
        }

        if (email.Length > MaxLength)
        {
            errors.Add(EmailError.TooLong);
        }

        if (!_regex.IsMatch(email))
        {
            errors.Add(EmailError.Invalid);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
