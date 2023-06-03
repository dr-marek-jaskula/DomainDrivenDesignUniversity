using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;

namespace Shopway.Persistence.Specifications.Products.Filtering;

public sealed record ProductDictionaryFilter : IFilter<Product>
{
    public string? LikeQuery { get; init; }
    private bool ByLikeQuery => LikeQuery.NotNullOrEmptyOrWhiteSpace();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Filter(ByLikeQuery, product => ((string)(object)product.ProductName).Contains(LikeQuery!)
                                         || ((string)(object)product.Revision).Contains(LikeQuery!));
    }
}