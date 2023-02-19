using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex _regex = new(@"^([1-9])[0-9]{8}$", RegexOptions.Compiled);
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static ValidationResult<PhoneNumber> Create(string number)
    {
        var errors = Validate(number);
        return errors.CreateValidationResult(() => new PhoneNumber(number));
    }

    public static List<Error> Validate(string number)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrEmpty(number))
        {
            errors.Add(PhoneNumberError.Empty);
        }

        if (!_regex.IsMatch(number))
        {
            errors.Add(PhoneNumberError.Invalid);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
