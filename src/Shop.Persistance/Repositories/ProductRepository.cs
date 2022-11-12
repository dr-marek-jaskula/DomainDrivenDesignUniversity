using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using Shopway.Persistence.Specifications.Products;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var specification = ProductByIdWithReviewsSpecification.Create(id);

        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Product, object>>[] includes)
    {
        var specification = ProductByIdWithIncludesSpecification.Create(id, includes);

        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
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
