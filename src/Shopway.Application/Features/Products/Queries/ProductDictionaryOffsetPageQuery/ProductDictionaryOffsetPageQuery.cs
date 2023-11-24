using Shopway.Domain.Common.DataProcessing;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products.DataProcessing.Sorting;
using Shopway.Domain.Products.DataProcessing.Filtering;

namespace Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;

public sealed record ProductDictionaryOffsetPageQuery(OffsetPage Page) : IOffsetPageQuery<DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDictionaryStaticFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}