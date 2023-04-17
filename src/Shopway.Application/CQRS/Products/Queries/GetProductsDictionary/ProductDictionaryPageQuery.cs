using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;

public sealed record ProductDictionaryPageQuery
(
    int PageNumber,
    int PageSize
)
    : IPageQuery<DictionaryResponseEntry, ProductDictionaryFilter, ProductOrder>
{
    public ProductDictionaryFilter? Filter { get; init; }
    public ProductOrder? Order { get; init; }
}