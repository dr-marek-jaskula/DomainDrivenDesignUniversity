using Shopway.Domain.BaseTypes;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record UserRegisteredDomainEvent(Guid Id, UserId UserId) : DomainEvent(Id)
{
    public static UserRegisteredDomainEvent New(UserId userId)
    {
        return new UserRegisteredDomainEvent(Guid.NewGuid(), userId);
    }
}