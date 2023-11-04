using Shopway.Domain.Common;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;

public sealed record ProductDictionaryOffsetPageQuery(OffsetPage Page) : IOffsetPageQuery<DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDictionaryStaticFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}