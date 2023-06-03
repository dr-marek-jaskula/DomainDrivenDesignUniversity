using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;

public sealed record ProductDictionaryPageQuery(Page Page) : IPageQuery<DictionaryResponseEntry, ProductDictionaryFilter, ProductOrder, Page>
{
    public ProductDictionaryFilter? Filter { get; init; }
    public ProductOrder? Order { get; init; }
}