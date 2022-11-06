using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using Shopway.Persistence.Specifications;
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

    /// <summary>
    /// An alternate way for specifications. Just include what we need - includes are specified as params
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public async Task<Product?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Product, object?>>[] includes)
    {
        var baseQuery = _dbContext
            .Set<Product>()
            .AsQueryable();

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                baseQuery = baseQuery.Include(include);
            }
        }

        Product? order = await baseQuery
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return order;
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
