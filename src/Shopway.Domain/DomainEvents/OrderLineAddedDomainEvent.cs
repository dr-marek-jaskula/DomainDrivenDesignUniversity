using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record OrderLineAddedDomainEvent(Ulid Id, OrderLineId OrderLineId, OrderHeaderId OrderHeaderId) : DomainEvent(Id)
{
    public static OrderLineAddedDomainEvent New(OrderLineId OrderLineId, OrderHeaderId OrderHeaderId)
    {
        return new OrderLineAddedDomainEvent(Ulid.NewUlid(), OrderLineId, OrderHeaderId);
    }
}