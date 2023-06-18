using Shopway.Domain.Entities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByKeyOrDefaultAsync(ProductKey productKey, CancellationToken cancellationToken);

    Task<bool> AnyAsync(ProductKey productKey, CancellationToken cancellationToken);

    Task<bool> AnyAsync(ProductId id, CancellationToken cancellationToken);

    /// <summary>
    /// Examines if the product ids refer to existing entities. Returns invalid reference
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Ids that do not refer to any product</returns>
    Task<IList<ProductId>> VerifyIdsAsync(IList<ProductId> ids, CancellationToken cancellationToken);

    Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken);

    Task<Product> GetByIdWithReviewAsync(ProductId id, ReviewId reviewId, CancellationToken cancellationToken);

    Task<Product> GetByIdWithReviewAsync(ProductId id, Title title, CancellationToken cancellationToken);

    Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes);

    Task<IDictionary<ProductKey, Product>> GetProductsDictionaryByNameAndRevision(IList<string> productNames, IList<string> productRevisions, IList<ProductKey> productKeys, Func<Product, ProductKey> toProductKey, CancellationToken cancellationToken);

    Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>(IPage page, IFilter<Product>? filter, ISortBy<Product>? sortBy, Expression<Func<Product, TResponse>>? select, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes);
    
    Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>(IPage page, IExpressionFilter<Product>? filter, ISortBy<Product>? sortBy, Expression<Func<Product, TResponse>>? select, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes);

    void Create(Product product);

    void Update(Product product);

    void Remove(ProductId id);
}