using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<Product>()
            .AsSplitQuery()
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

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
