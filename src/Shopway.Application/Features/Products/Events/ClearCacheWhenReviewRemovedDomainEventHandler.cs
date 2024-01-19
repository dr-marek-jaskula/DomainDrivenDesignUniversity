using Shopway.Application.Abstractions;
using Shopway.Application.Utilities;
using Shopway.Domain.Products.Events;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Application.Features.Products.Events;

internal sealed class ClearCacheWhenReviewRemovedDomainEventHandler(IFusionCache fusionCache) : IDomainEventHandler<ReviewRemovedDomainEvent>
{
    private readonly IFusionCache _fusionCache = fusionCache;

    public Task Handle(ReviewRemovedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _fusionCache.Remove(domainEvent.ReviewId);
        return Task.CompletedTask;
    }
}
