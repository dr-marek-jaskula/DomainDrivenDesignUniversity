using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using System.Text.RegularExpressions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static System.Text.RegularExpressions.RegexOptions;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class Password : ValueObject
{
    private static readonly Regex _regex = new(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,}$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));

    public const int MaxLength = 30;
    public const int MinLength = 9;
    public new string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static ValidationResult<Password> Create(string password)
    {
        var errors = Validate(password);
        return errors.CreateValidationResult(() => new Password(password));
    }

    public static IList<Error> Validate(string password)
    {
        return EmptyList<Error>()
            .If(password.IsNullOrEmptyOrWhiteSpace(), PasswordError.Empty)
            .If(password.Length < MinLength, PasswordError.TooShort)
            .If(password.Length > MaxLength, PasswordError.TooLong)
            .If(_regex.NotMatch(password), PasswordError.Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}