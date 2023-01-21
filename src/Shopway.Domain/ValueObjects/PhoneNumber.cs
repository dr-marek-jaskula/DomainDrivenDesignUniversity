using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using System.Text.RegularExpressions;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex _regex = new(@"^([1-9])[0-9]{8}$", RegexOptions.Compiled);
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return Result.Failure<PhoneNumber>(PhoneNumberError.Empty);
        }

        if (!_regex.IsMatch(number))
        {
            return Result.Failure<PhoneNumber>(PhoneNumberError.Invalid);
        }

        return new PhoneNumber(number);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
