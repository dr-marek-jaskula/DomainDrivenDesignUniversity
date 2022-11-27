using Shopway.Domain.Entities;
using Shopway.Domain.StronglyTypedIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);

    Task<Product?> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken = default, params Expression<Func<Product, object>>[] includes);

    void Create(Product order);

    void Update(Product order);

    void Remove(Product product);
}