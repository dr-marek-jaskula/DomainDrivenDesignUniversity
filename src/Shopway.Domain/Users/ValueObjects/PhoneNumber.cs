using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Users.ValueObjects;

public sealed record class PhoneNumber : ValueObject<string>
{
    private static readonly Regex _regex = new(@"^([1-9])[0-9]{8}$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));

    public static readonly Error Empty = Error.New(
        $"{nameof(PhoneNumber)}.{nameof(Empty)}",
        $"{nameof(PhoneNumber)} is empty.");

    public static readonly Error Invalid = Error.New(
        $"{nameof(PhoneNumber)}.{nameof(Invalid)}",
        $"{nameof(PhoneNumber)} must consist of 9 digits and cannot start from zero.");

    private PhoneNumber(string phoneNumber) : base(phoneNumber)
    {
    }

    public static ValidationResult<PhoneNumber> Create(string number)
    {
        var errors = Validate(number);
        return errors.CreateValidationResult(() => new PhoneNumber(number));
    }

    public static IList<Error> Validate(string number)
    {
        return EmptyList<Error>()
            .If(number.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(_regex.NotMatch(number), Invalid);
    }
}
