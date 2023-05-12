﻿using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications;
using Shopway.Persistence.Specifications.Products;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class ProductRepository : RepositoryBase, IProductRepository
{
    public ProductRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product?> GetByKeyOrDefaultAsync(ProductKey key, CancellationToken cancellationToken)
    {
        var specification = ProductByKeyQuerySpecification.Create(key);

        return await UseSpecificationWithoutMapping(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        var specification = ProductByKeyQuerySpecification.Create(key);

        return await UseSpecificationWithoutMapping(specification)
            .AnyAsync(cancellationToken);
    }

    public async Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        var specification = ProductByIdWithReviewsQuerySpecification.Create(id);

        return await UseSpecificationWithoutMapping(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
    {
        var specification = ProductByIdWithIncludesQuerySpecification.Create(id, includes);

        return await UseSpecificationWithoutMapping(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IPage page, 
        IFilter<Product>? filter, 
        ISortBy<Product>? sort, 
        Expression<Func<Product, TResponse>>? select, 
        CancellationToken cancellationToken, 
        params Expression<Func<Product, object>>[] includes
    )
    {
        var specification = CommonQuerySpecificationWithMapping<Product, ProductId, TResponse>.Create(filter, sort, select, includes);
        
        return await UseSpecificationWithMapping(specification)
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
