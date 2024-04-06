using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class TwoFactorToptSecret : ValueObject
{
    public const int BytesLong = 32;

    private TwoFactorToptSecret(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<TwoFactorToptSecret> Create(string twoFactorToptSecret)
    {
        var errors = Validate(twoFactorToptSecret);
        return errors.CreateValidationResult(() => new TwoFactorToptSecret(twoFactorToptSecret));
    }

    public static IList<Error> Validate(string twoFactorTokenHash)
    {
        return EmptyList<Error>()
            .If(twoFactorTokenHash.IsNullOrEmptyOrWhiteSpace(), TwoFactorToptSecretError.Empty)
            .If(Encoding.ASCII.GetByteCount(twoFactorTokenHash) != BytesLong, TwoFactorToptSecretError.BytesLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}