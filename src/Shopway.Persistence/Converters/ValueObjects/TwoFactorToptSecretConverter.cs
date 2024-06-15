using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class TwoFactorToptSecretConverter : ValueConverter<TwoFactorToptSecret, string>
{
    public TwoFactorToptSecretConverter() : base(twoFactorToptSecret => twoFactorToptSecret.Value, @string => TwoFactorToptSecret.Create(@string).Value) { }
}

public sealed class TwoFactorToptSecretComparer : ValueComparer<TwoFactorToptSecret>
{
    public TwoFactorToptSecretComparer() : base((twoFactorToptSecret1, twoFactorToptSecret2) => twoFactorToptSecret1!.Value == twoFactorToptSecret2!.Value, twoFactorToptSecret => twoFactorToptSecret.GetHashCode()) { }
}
