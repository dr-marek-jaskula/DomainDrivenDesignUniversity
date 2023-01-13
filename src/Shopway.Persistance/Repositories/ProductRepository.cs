using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications.Products;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        var specification = ProductByIdWithReviewsQuerySpecification.Create(id);

        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken = default, params Expression<Func<Product, object>>[] includes)
    {
        var specification = ProductByIdWithIncludesQuerySpecification.Create(id, includes);

        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<Product> Queryable(IFilter<Product>? filter, ISortBy? sort)
    {
        var specification = ProductQuerySpecification.Create(filter, sort);

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
