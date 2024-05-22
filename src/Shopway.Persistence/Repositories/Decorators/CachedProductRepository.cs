using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedProductRepository(IProductRepository decorated, IFusionCache fusionCache, ShopwayDbContext dbContext) : IProductRepository
{
    private readonly IProductRepository _decorated = decorated;
    private readonly IFusionCache _fusionCache = fusionCache;
    private readonly ShopwayDbContext _dbContext = dbContext;

    public async Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        var product = await _fusionCache.GetOrSetAsync
        (
            id.ToCacheKey(),
            cancellationToken => _decorated.GetByIdAsync(id, cancellationToken)!,
            TimeSpan.FromSeconds(30),
            cancellationToken
        );

        return _dbContext.AttachToChangeTrackerWhenTrackingBehaviorIsDifferentFromNoTracking(product);
    }

    public Task<IList<Product>> GetByIdsAsync(IList<ProductId> ids, CancellationToken cancellationToken)
    {
        return _decorated.GetByIdsAsync(ids, cancellationToken);
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
        //This would not be required, but to demonstrate the "ExecuteDelete" technique, it needs to be done here, since ChangeTracker is not updated when using "ExecuteDelete"
        _fusionCache.Remove(id);
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

    public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IOffsetPage page, 
        CancellationToken cancellationToken, 
        IFilter<Product>? filter = null,
        IList<LikeEntry<Product>>? likes = null,
        ISortBy<Product>? sort = null,
        IMapping<Product, TResponse>? mapping = null,
        Expression<Func<Product, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<Product>>? buildIncludes = null
    )
    {
        return await _decorated.PageAsync
        (
            page,
            cancellationToken,
            filter: filter,
            likes: likes,
            sort: sort,
            mapping: mapping,
            mappingExpression: mappingExpression,
            buildIncludes: buildIncludes
        );
    }

    public async Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        ICursorPage page, 
        CancellationToken cancellationToken, 
        IFilter<Product>? filter = null,
        IList<LikeEntry<Product>>? likes = null,
        ISortBy<Product>? sort = null,
        IMapping<Product, TResponse>? mapping = null,
        Expression<Func<Product, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<Product>>? buildIncludes = null
    )
        where TResponse : class, IHasCursor
    {
        return await _decorated.PageAsync
        (
            page,
            cancellationToken,
            filter: filter,
            likes: likes,
            sort: sort,
            mapping: mapping,
            mappingExpression: mappingExpression,
            buildIncludes: buildIncludes
        );
    }

    public async Task<Product> QueryByIdAsync
    (
        ProductId productId, 
        CancellationToken cancellationToken, 
        Action<IIncludeBuilder<Product>>? buildIncludes = null
    )
    {
        return await _decorated.QueryByIdAsync
        (
            productId,
            cancellationToken,
            buildIncludes
        );
    }

    public async Task<TResponse> QueryByIdAsync<TResponse>
    (
        ProductId productId,
        CancellationToken cancellationToken,
        IMapping<Product, TResponse>? mapping = null
    )
        where TResponse : class
    {
        return await _decorated.QueryByIdAsync
        (
            productId,
            cancellationToken,
            mapping
        );
    }
}
