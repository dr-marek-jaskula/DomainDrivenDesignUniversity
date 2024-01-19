using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products.ValueObjects;
using System.Linq.Expressions;

namespace Shopway.Domain.Products;

public interface IProductRepository
{
    Task<IList<string>> GetNamesAsync(CancellationToken cancellationToken);

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

    Task<IList<Product>> GetByIdsAsync(IList<ProductId> ids, CancellationToken cancellationToken);

    Task<Product> GetByIdWithReviewAsync(ProductId id, ReviewId reviewId, CancellationToken cancellationToken);

    Task<Product> GetByIdWithReviewAsync(ProductId id, Title title, CancellationToken cancellationToken);

    Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes);

    Task<IDictionary<ProductKey, Product>> GetProductsDictionaryByNameAndRevision(IList<string> productNames, IList<string> productRevisions, IList<ProductKey> productKeys, Func<Product, ProductKey> toProductKey, CancellationToken cancellationToken);

    Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IOffsetPage page,
        CancellationToken cancellationToken,
        IFilter<Product>? filter = null,
        ISortBy<Product>? sort = null,
        Expression<Func<Product, TResponse>>? mapping = null,
        params Expression<Func<Product, object>>[] includes
    );

    Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        ICursorPage page,
        CancellationToken cancellationToken,
        IFilter<Product>? filter = null,
        ISortBy<Product>? sort = null,
        Expression<Func<Product, TResponse>>? mapping = null,
        params Expression<Func<Product, object>>[] includes
    )
        where TResponse : class, IHasCursor;

    void Create(Product product);

    void Update(Product product);

    void Remove(ProductId id);
}