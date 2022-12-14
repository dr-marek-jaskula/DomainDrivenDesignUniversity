using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record ProductCreatedDomainEvent(Guid Id, ProductId ProductId) : DomainEvent(Id)
{
    public static ProductCreatedDomainEvent New(ProductId ProductId)
    {
        return new ProductCreatedDomainEvent(Guid.NewGuid(), ProductId);
    }
}