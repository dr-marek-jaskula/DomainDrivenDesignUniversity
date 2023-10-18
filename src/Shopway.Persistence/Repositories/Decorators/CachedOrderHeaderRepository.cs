using Shopway.Domain.Entities;
using System.Linq.Expressions;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using ZiggyCreatures.Caching.Fusion;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedOrderHeaderRepository : IOrderHeaderRepository
{
    private readonly IOrderHeaderRepository _decorated;
    private readonly IFusionCache _fusionCache;
    private readonly ShopwayDbContext _context;

    public CachedOrderHeaderRepository(IOrderHeaderRepository decorated, IFusionCache fusionCache, ShopwayDbContext context)
    {
        _decorated = decorated;
        _fusionCache = fusionCache;
        _context = context;
    }

    public async Task<OrderHeader> GetByIdAsync(OrderHeaderId id, CancellationToken cancellationToken)
    {
        var order = await _fusionCache.GetOrSetAsync
        (
            id.ToCacheKey(),
            cancellationToken => _decorated.GetByIdAsync(id, cancellationToken)!,
            TimeSpan.FromSeconds(30),
            cancellationToken
        );

        return _context.AttachToChangeTrackerWhenTrackingBehaviorIsDifferentFromNoTracking(order);
    }

    public Task<OrderHeader> GetByIdWithIncludesAsync(OrderHeaderId id, CancellationToken cancellationToken, params Expression<Func<OrderHeader, object>>[] includes)
    {
        return _decorated.GetByIdWithIncludesAsync(id, cancellationToken, includes);
    }

    public void Create(OrderHeader order)
    {
        _decorated.Create(order);
    }

    public void Remove(OrderHeader order)
    {
        _decorated.Remove(order);
    }

    public void Update(OrderHeader order)
    {
        _decorated.Update(order);
        _fusionCache.Update<OrderHeader, OrderHeaderId>(order);
    }

    public async Task<OrderHeader> GetByIdWithOrderLineAsync(OrderHeaderId id, OrderLineId orderLineId, CancellationToken cancellationToken)
    {
        return await _decorated.GetByIdWithOrderLineAsync(id, orderLineId, cancellationToken);
    }
}