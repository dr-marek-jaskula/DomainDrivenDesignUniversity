using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class Email : ValueObject
{
    public const int MaxLength = 40;

    private static readonly Regex _regex = new(@"^([a-zA-Z])([a-zA-Z0-9]+)\.?([a-zA-Z0-9]+)@([a-z]+)\.[a-z]{2,3}$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));

    public static readonly Error Empty = Error.New(
        $"{nameof(Email)}.{nameof(Empty)}",
        $"{nameof(Email)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(Email)}.{nameof(TooLong)}",
        $"{nameof(Email)} must be at most {MaxLength} characters long.");

    public static readonly Error Invalid = Error.New(
        $"{nameof(Email)}.{nameof(Invalid)}",
        $"{nameof(Email)} must start from a letter, contain '@' and after that '.'.");

    public static readonly Error AlreadyTaken = Error.New(
        $"{nameof(Email)}.{nameof(AlreadyTaken)}",
        $"{nameof(Email)} is already taken.");


    private Email(string value)
    {
        Value = value;
    }

    private Email()
    {
    }

    public new string Value { get; }

    public static ValidationResult<Email> Create(string email)
    {
        var errors = Validate(email);
        return errors.CreateValidationResult(() => new Email(email));
    }

    public static IList<Error> Validate(string email)
    {
        return EmptyList<Error>()
            .If(email.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(email.Length > MaxLength, TooLong)
            .If(_regex.NotMatch(email), Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
