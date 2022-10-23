using Microsoft.Extensions.Caching.Memory;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedOrderRepository : IOrderRepository
{
    private readonly IOrderRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedOrderRepository(IOrderRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"order-{id}";

        return _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                return _decorated.GetByIdAsync(id, cancellationToken);
            });
    }

    public Task<Order?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Order, object?>>[] includes)
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

