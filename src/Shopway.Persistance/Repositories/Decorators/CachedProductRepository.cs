using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;
using Shopway.Persistence.Framework;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.EntityBusinessKeys;
using ZiggyCreatures.Caching.Fusion;
using Shopway.Persistence.Utilities;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.BaseTypes;
using System.Collections.Generic;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _decorated;
    private readonly IFusionCache _fusionCache;
    private readonly ShopwayDbContext _context;

    public CachedProductRepository(IProductRepository decorated, IFusionCache fusionCache, ShopwayDbContext context)
    {
        _decorated = decorated;
        _fusionCache = fusionCache;
        _context = context;
    }

    public async Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        var product = await _fusionCache.GetOrSetAsync
        (
            id.ToCacheKey(),
            cancellationToken => _decorated.GetByIdAsync(id, cancellationToken)!,
            TimeSpan.FromSeconds(30),
            cancellationToken
        );

        return _context.AttachToChangeTrackerWhenTrackingBehaviorIsDifferentFromNoTracking(product);
    }

    public Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
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

    public Task<Product> GetByKeyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        return _decorated.GetByKeyAsync(key, cancellationToken);
    }

    public Task<bool> AnyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        return _decorated.AnyAsync(key, cancellationToken);
    }

    public Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>(IPage page, IFilter<Product>? filter, ISortBy<Product>? sortBy, Expression<Func<Product, TResponse>>? select, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
    {
        return _decorated.PageAsync(page, filter, sortBy, select, cancellationToken, includes);
    }
}
