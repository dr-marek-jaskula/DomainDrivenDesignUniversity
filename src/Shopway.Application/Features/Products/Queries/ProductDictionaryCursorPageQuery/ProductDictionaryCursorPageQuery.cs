using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

public sealed record ProductDictionaryCursorPageQuery(CursorPage Page)
    : ICursorPageQuery<DictionaryResponseEntry<ProductKey>, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    public ProductDictionaryStaticFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}
