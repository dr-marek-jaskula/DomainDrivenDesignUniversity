using Microsoft.Extensions.Caching.Memory;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedProductRepository(IProductRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"product-{id}";

        return _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                return _decorated.GetByIdAsync(id, cancellationToken);
            });
    }

    public Task<Product?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Product, object?>>[] includes)
    {
        return _decorated.GetByIdWithIncludesAsync(id, cancellationToken, includes);
    }

    public void Create(Product product)
    {
        _decorated.Create(product);
    }

    public void Remove(Product product)
    {
        _decorated.Remove(product);
    }

    public void Update(Product product)
    {
        _decorated.Update(product);
    }
}

