﻿using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications;
using Shopway.Persistence.Specifications.Common;
using Shopway.Persistence.Specifications.Products;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;
using static Shopway.Domain.Common.Utilities.StringUtilities;

namespace Shopway.Persistence.Repositories;

public sealed class ProductRepository(ShopwayDbContext dbContext) : IProductRepository
{
    private readonly ShopwayDbContext _dbContext = dbContext;

    public async Task<IList<string>> GetNamesAsync(CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.Names.Create();

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByKeyOrDefaultAsync(ProductKey productKey, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ByKey.Create(productKey);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ProductKey productKey, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ByKey.Create(productKey);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ProductId id, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ById.Create(id);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .AnyAsync(cancellationToken);
    }

    public async Task<IList<ProductId>> VerifyIdsAsync(IList<ProductId> ids, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ById.Create(ids, product => product.Id);

        var existing = await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .ToListAsync(cancellationToken);

        return ids.Except(existing).ToList();
    }

    public async Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ById.WithReviews.Create(id);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<IList<Product>> GetByIdsAsync(IList<ProductId> ids, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ById.Create(ids);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> GetByIdWithReviewAsync(ProductId id, ReviewId reviewId, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ById.WithReview.Create(id, reviewId);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<Product> GetByIdWithReviewAsync(ProductId id, Title title, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ById.WithReview.Create(id, title);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
    {
        var specification = ProductSpecification.ById.WithIncludes.Create(id, includes);

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<IDictionary<ProductKey, Product>> GetProductsDictionaryByNameAndRevision(IList<string> productNames, IList<string> productRevisions, IList<ProductKey> productKeys, Func<Product, ProductKey> toProductKey, CancellationToken cancellationToken)
    {
        var specification = ProductSpecification.ByNamesAndRevisions.Create(productNames, productRevisions);

        //We query too many products: all combinations of ProductName and Revision. Therefore, we will need to filter them
        var productsToFilter = await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .ToListAsync(cancellationToken);

        return productsToFilter
            .Where(product => productKeys.Any(key => key.ProductName.CaseInsensitiveEquals(product.ProductName.Value)
                                                  && key.Revision.CaseInsensitiveEquals(product.Revision.Value)))
            .ToDictionary(product => toProductKey(product));
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
        var specification = CommonSpecification.WithMapping.Create<Product, ProductId, TResponse>
        (
            filter,
            null,
            likes,
            sort,
            mapping,
            mappingExpression,
            buildIncludes: buildIncludes
        );

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .PageAsync(page, cancellationToken);
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
        Expression<Func<Product, bool>> cursorFilter = product => product.Id >= ProductId.Create(page.Cursor);

        var specification = CommonSpecification.WithMapping.Create<Product, ProductId, TResponse>
        (
            filter,
            cursorFilter,
            likes,
            sort,
            mapping,
            mappingExpression,
            buildIncludes: buildIncludes
        );

        return await _dbContext
            .Set<Product>()
            .UseSpecification(specification)
            .PageAsync(page, cancellationToken);
    }

    public void Create(Product product)
    {
        _dbContext
            .Set<Product>()
            .Add(product);
    }

    public void Update(Product product)
    {
        _dbContext
            .Set<Product>()
            .Update(product);
    }

    public void Remove(ProductId id)
    {
        _dbContext.Set<Product>()
            .Where(product => product.Id == id)
            .ExecuteDelete();
    }
}
