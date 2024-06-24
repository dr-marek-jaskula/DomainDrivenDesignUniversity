using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class ExternalIdentityProviderConverter : ValueConverter<ExternalIdentityProvider, string>
{
    public ExternalIdentityProviderConverter() : base(externalIdentityProvider => externalIdentityProvider.Value, @decimal => ExternalIdentityProvider.Create(@decimal).Value) { }
}

public sealed class ExternalIdentityProviderComparer : ValueComparer<ExternalIdentityProvider>
{
    public ExternalIdentityProviderComparer() : base((externalIdentityProvider1, externalIdentityProvider2) => externalIdentityProvider1!.Value == externalIdentityProvider2!.Value, price => price.GetHashCode()) { }
}
