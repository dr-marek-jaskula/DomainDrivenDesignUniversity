using Shopway.Domain.Common.BaseTypes;

namespace Shopway.Domain.Users.Events;

public sealed record TwoFactorTokenCreatedDomainEvent(Ulid Id, UserId UserId, string TwoFactorToken) : DomainEvent(Id)
{
    public static TwoFactorTokenCreatedDomainEvent New(UserId userId, string twoFactorToken)
    {
        return new TwoFactorTokenCreatedDomainEvent(Ulid.NewUlid(), userId, twoFactorToken);
    }
}
