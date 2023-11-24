using Shopway.Domain.Common.BaseTypes;

namespace Shopway.Domain.Products.Events;

public sealed record ReviewRemovedDomainEvent(Ulid Id, ReviewId ReviewId, ProductId ProductId) : DomainEvent(Id)
{
    public static ReviewRemovedDomainEvent New(ReviewId ReviewId, ProductId ProductId)
    {
        return new ReviewRemovedDomainEvent(Ulid.NewUlid(), ReviewId, ProductId);
    }
}