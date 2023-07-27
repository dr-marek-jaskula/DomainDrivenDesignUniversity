﻿using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record OrderHeaderCreatedDomainEvent(Ulid Id, OrderHeaderId OrderId) : DomainEvent(Id)
{
    public static OrderHeaderCreatedDomainEvent New(OrderHeaderId OrderId)
    {
        return new OrderHeaderCreatedDomainEvent(Ulid.NewUlid(), OrderId);
    }
}
