using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedOrderRepository : IOrderRepository
{
    private readonly IOrderRepository _decorated;
    private readonly IFusionCache _fusionCache;
    private readonly DbSet<Order> _orderDbSet;

    public CachedOrderRepository(IOrderRepository decorated, IFusionCache fusionCache, ShopwayDbContext context)
    {
        _decorated = decorated;
        _fusionCache = fusionCache;
        _orderDbSet = context.Set<Order>();
    }

    public async Task<Order> GetByIdAsync(OrderId id, CancellationToken cancellationToken)
    {
        var order = await _fusionCache.GetOrSetAsync
        (
            id.ToCacheKey(),
            cancellationToken => _decorated.GetByIdAsync(id, cancellationToken)!,
            TimeSpan.FromSeconds(30),
            cancellationToken
        );

        return _orderDbSet.AttachAndReturn(order);
    }

    public Task<Order> GetByIdWithIncludesAsync(OrderId id, CancellationToken cancellationToken, params Expression<Func<Order, object>>[] includes)
    {
        return _decorated.GetByIdWithIncludesAsync(id, cancellationToken, includes);
    }

    public void Create(Order order)
    {
        _decorated.Create(order);
    }

    public void Remove(Order order)
    {
        _decorated.Remove(order);
    }

    public void Update(Order order)
    {
        _decorated.Update(order);
    }
}

