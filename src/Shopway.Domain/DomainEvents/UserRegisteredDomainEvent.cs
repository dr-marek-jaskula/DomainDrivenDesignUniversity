using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record UserRegisteredDomainEvent(Ulid Id, UserId UserId) : DomainEvent(Id)
{
    public static UserRegisteredDomainEvent New(UserId userId)
    {
        return new UserRegisteredDomainEvent(Ulid.NewUlid(), userId);
    }
}