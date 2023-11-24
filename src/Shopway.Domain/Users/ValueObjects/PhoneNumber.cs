using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using System.Text.RegularExpressions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

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
