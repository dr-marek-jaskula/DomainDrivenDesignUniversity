using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed record class ExternalIdentityProvider : ValueObject<string>
{
    public const string GoogleExternalIdentityProvider = "Google";
    public static readonly FrozenSet<string> LegalIdentityProviders = AsList
    (
        GoogleExternalIdentityProvider
    ).ToFrozenSet();

    public static readonly Error Empty = Error.New(
        $"{nameof(ExternalIdentityProvider)}.{nameof(Empty)}",
        $"{nameof(ExternalIdentityProvider)} is empty.");

    public static readonly Error Invalid = Error.New(
        $"{nameof(ExternalIdentityProvider)}.{nameof(Invalid)}",
        $"{nameof(ExternalIdentityProvider)} be valid identity provider");

    private ExternalIdentityProvider(string externalIdentityProvider) : base(externalIdentityProvider)
    {
    }

    public static ValidationResult<ExternalIdentityProvider> Create(string externalIdentityProvider)
    {
        var errors = Validate(externalIdentityProvider);
        return errors.CreateValidationResult(() => new ExternalIdentityProvider(externalIdentityProvider));
    }

    public static IList<Error> Validate(string externalIdentityProvider)
    {
        return EmptyList<Error>()
            .If(externalIdentityProvider.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(LegalIdentityProviders.Contains(externalIdentityProvider) is false, Invalid);
    }
}
