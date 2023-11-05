using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Filering.Products;

public sealed record ProductDictionaryStaticFilter : IFilter<Product>
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