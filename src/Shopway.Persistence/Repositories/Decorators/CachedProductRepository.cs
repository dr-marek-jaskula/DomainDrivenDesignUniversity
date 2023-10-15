using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;
using Shopway.Persistence.Framework;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.EntityKeys;
using ZiggyCreatures.Caching.Fusion;
using Shopway.Persistence.Utilities;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions.Common;

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
        //_fusionCache.Set<Product, ProductId>(product); //Not required because cache update is done in UnitOfWork. Can be used in specific cases
    }

    public void Remove(ProductId id)
    {
        _decorated.Remove(id);
        //_fusionCache.Remove(id); //Not required because cache update is done in UnitOfWork. Can be used in specific cases
    }

    public void Update(Product product)
    {
        _decorated.Update(product);
        _fusionCache.Update<Product, ProductId>(product);
    }

    public async Task<IList<string>> GetNamesAsync(CancellationToken cancellationToken)
    {
        return await _decorated.GetNamesAsync(cancellationToken);
    }

    public Task<Product?> GetByKeyOrDefaultAsync(ProductKey productKey, CancellationToken cancellationToken)
    {
        return _decorated.GetByKeyOrDefaultAsync(productKey, cancellationToken);
    }

    public Task<bool> AnyAsync(ProductId id, CancellationToken cancellationToken)
    {
        return _decorated.AnyAsync(id, cancellationToken);
    }

    public Task<bool> AnyAsync(ProductKey productKey, CancellationToken cancellationToken)
    {
        return _decorated.AnyAsync(productKey, cancellationToken);
    }

    public Task<IDictionary<ProductKey, Product>> GetProductsDictionaryByNameAndRevision(IList<string> productNames, IList<string> productRevisions, IList<ProductKey> productKeys, Func<Product, ProductKey> toProductKey, CancellationToken cancellationToken)
    {
        return _decorated.GetProductsDictionaryByNameAndRevision(productNames, productRevisions, productKeys, toProductKey, cancellationToken);
    }

    public Task<Product> GetByIdWithReviewAsync(ProductId id, ReviewId reviewId, CancellationToken cancellationToken)
    {
        return _decorated.GetByIdWithReviewAsync(id, reviewId, cancellationToken);
    }

    public Task<Product> GetByIdWithReviewAsync(ProductId id, Title title, CancellationToken cancellationToken)
    {
        return _decorated.GetByIdWithReviewAsync(id, title, cancellationToken);
    }

    public Task<IList<ProductId>> VerifyIdsAsync(IList<ProductId> ids, CancellationToken cancellationToken)
    {
        return _decorated.VerifyIdsAsync(ids, cancellationToken);
    }

    public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>(IOffsetPage page, CancellationToken cancellationToken, IDynamicFilter<Product>? dynamicFilter = null, IStaticFilter<Product>? staticFilter = null, IStaticSortBy<Product>? staticSort = null, IDynamicSortBy<Product>? dynamicSort = null, Expression<Func<Product, TResponse>>? mapping = null, params Expression<Func<Product, object>>[] includes)
    {
        return await _decorated.PageAsync
        (
            page,
            cancellationToken,
            dynamicFilter: dynamicFilter,
            staticFilter: staticFilter,
            staticSort: staticSort,
            dynamicSort: dynamicSort,
            mapping: mapping,
            includes: includes
        );
    }

    public async Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>(ICursorPage page, CancellationToken cancellationToken, IDynamicFilter<Product>? dynamicFilter = null, IStaticFilter<Product>? staticFilter = null, IStaticSortBy<Product>? staticSort = null, IDynamicSortBy<Product>? dynamicSort = null, Expression<Func<Product, TResponse>>? mapping = null, params Expression<Func<Product, object>>[] includes)
        where TResponse : class, IHasCursor
    {
        return await _decorated.PageAsync
        (
            page,
            cancellationToken,
            dynamicFilter: dynamicFilter,
            staticFilter: staticFilter,
            staticSort: staticSort,
            dynamicSort: dynamicSort,
            mapping: mapping,
            includes: includes
        );
    }
}
