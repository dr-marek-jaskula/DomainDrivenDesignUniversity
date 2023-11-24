using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Orders;

namespace Shopway.Domain.DomainEvents;

public sealed record OrderHeaderCreatedDomainEvent(Ulid Id, OrderHeaderId OrderId) : DomainEvent(Id)
{
    public static OrderHeaderCreatedDomainEvent New(OrderHeaderId OrderId)
    {
        return new OrderHeaderCreatedDomainEvent(Ulid.NewUlid(), OrderId);
    }
}
