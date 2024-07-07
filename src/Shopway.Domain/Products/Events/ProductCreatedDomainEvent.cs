using Shopway.Domain.Common.BaseTypes;
using System.Text.Json;
namespace Shopway.Domain.Products.Events;

public sealed record ProductCreatedDomainEvent(Ulid Id, ProductId ProductId) : DomainEvent(Id)
{
    public static ProductCreatedDomainEvent New(ProductId ProductId)
    {
        return new ProductCreatedDomainEvent(Ulid.NewUlid(), ProductId);
    }
}
