using Shopway.Domain.Common.DataProcessing;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products.DataProcessing.Sorting;
using Shopway.Domain.Products.DataProcessing.Filtering;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

public sealed record ProductDictionaryCursorPageQuery(CursorPage Page) : ICursorPageQuery<DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    public ProductDictionaryStaticFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}