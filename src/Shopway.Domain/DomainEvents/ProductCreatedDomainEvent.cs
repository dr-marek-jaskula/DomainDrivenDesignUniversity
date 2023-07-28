using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record ProductCreatedDomainEvent(Ulid Id, ProductId ProductId) : DomainEvent(Id)
{
    public static ProductCreatedDomainEvent New(ProductId ProductId)
    {
        return new ProductCreatedDomainEvent(Ulid.NewUlid(), ProductId);
    }
}