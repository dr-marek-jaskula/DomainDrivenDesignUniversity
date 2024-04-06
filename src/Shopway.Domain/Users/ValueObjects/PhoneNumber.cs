using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex _regex = new(@"^([1-9])[0-9]{8}$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));
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
