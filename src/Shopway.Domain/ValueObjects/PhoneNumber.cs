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
    public new string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static ValidationResult<PhoneNumber> Create(string number)
    {
        var errors = Validate(number);
        return errors.CreateValidationResult(() => new PhoneNumber(number));
    }

    public static IList<Error> Validate(string number)
    {
        return EmptyList<Error>()
            .If(number.IsNullOrEmptyOrWhiteSpace(), PhoneNumberError.Empty)
            .If(_regex.NotMatch(number), PhoneNumberError.Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
