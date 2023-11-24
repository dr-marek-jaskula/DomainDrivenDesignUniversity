using Shopway.Application.Utilities;
using ZiggyCreatures.Caching.Fusion;
using Shopway.Domain.Products.Events;
using Shopway.Application.Abstractions;

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
