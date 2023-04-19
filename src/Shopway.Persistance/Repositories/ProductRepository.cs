using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications.Products;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class ProductRepository : RepositoryBase, IProductRepository
{
    public ProductRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product> GetByKeyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        var specification = ProductByKeyQuerySpecification.Create(key);

        return await ApplySpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        var specification = ProductByKeyQuerySpecification.Create(key);

        return await ApplySpecification(specification)
            .AnyAsync(cancellationToken);
    }

    public async Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        var specification = ProductByIdWithReviewsQuerySpecification.Create(id);

        return await ApplySpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
    {
        var specification = ProductByIdWithIncludesQuerySpecification.Create(id, includes);

        return await ApplySpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public IQueryable<Product> Queryable(IFilter<Product>? filter, ISortBy<Product>? sort, params Expression<Func<Product, object>>[] includes)
    {
        var specification = ProductQuerySpecification.Create(filter, sort, includes);

        return ApplySpecification(specification);
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

    public void Remove(Product product)
    {
        var entry = _dbContext
            .Set<Product>()
            .Attach(product);

        entry.State = EntityState.Deleted;
    }
}
