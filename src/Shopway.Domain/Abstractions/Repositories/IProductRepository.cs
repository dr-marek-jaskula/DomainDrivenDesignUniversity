using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IProductRepository
{
    Task<Product> GetByKeyAsync(ProductKey key, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(ProductKey key, CancellationToken cancellationToken = default);

    Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);

    Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken = default, params Expression<Func<Product, object>>[] includes);

    IQueryable<Product> Queryable(IFilter<Product>? filter, ISortBy<Product>? sortBy);

    void Create(Product order);

    void Update(Product order);

    void Remove(Product product);
}