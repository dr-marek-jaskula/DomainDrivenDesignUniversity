using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using System.Text.RegularExpressions;

namespace Shopway.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex _regex = new(@"^([1-9])[0-9]{8}$",
    //This option boots the performance of the regex
    RegexOptions.Compiled);
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    } 

    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return Result.Failure<PhoneNumber>(DomainErrors.PhoneNumberError.Empty);
        }

        if (!_regex.IsMatch(number))
        {
            return Result.Failure<PhoneNumber>(DomainErrors.PhoneNumberError.Invalid);
        }

        return new PhoneNumber(number);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
