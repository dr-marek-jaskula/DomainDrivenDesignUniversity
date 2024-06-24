using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class Password : ValueObject
{
    private static readonly Regex _regex = new(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,}$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));

    public const int MaxLength = 30;
    public const int MinLength = 9;

    public static readonly Error Empty = Error.New(
        $"{nameof(Password)}.{nameof(Empty)}",
        $"{nameof(Password)} is empty.");

    public static readonly Error TooShort = Error.New(
        $"{nameof(Password)}.{nameof(TooShort)}",
        $"{nameof(Password)} needs to be at least {MinLength} characters long.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(Password)}.{nameof(TooLong)}",
        $"{nameof(Password)} needs to be at most {MaxLength} characters long.");

    public static readonly Error Invalid = Error.New(
        $"{nameof(Password)}.{nameof(Invalid)}",
        $"{nameof(Password)} needs to contain at least one digit, one small letter, one capital letter and one special character.");

    private Password(string value)
    {
        Value = value;
    }

    private Password()
    {
    }

    public new string Value { get; }

    public static ValidationResult<Password> Create(string password)
    {
        var errors = Validate(password);
        return errors.CreateValidationResult(() => new Password(password));
    }

    public static ValidationResult<Password> CreateRandomPassword()
    {
        var randomPassword = $"aA1!{Ulid.NewUlid()}";

        if (randomPassword.Length > MaxLength)
        {
            randomPassword = randomPassword[..MaxLength];
        }

        return Create(randomPassword);
    }

    public static IList<Error> Validate(string password)
    {
        return EmptyList<Error>()
            .If(password.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(password.Length < MinLength, TooShort)
            .If(password.Length > MaxLength, TooLong)
            .If(_regex.NotMatch(password), Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
