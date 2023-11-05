using Shopway.Domain.Common;
using Shopway.Application.Sorting.Products;
using Shopway.Application.Filering.Products;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

public sealed record ProductDictionaryCursorPageQuery(CursorPage Page) : ICursorPageQuery<DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    public ProductDictionaryStaticFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}