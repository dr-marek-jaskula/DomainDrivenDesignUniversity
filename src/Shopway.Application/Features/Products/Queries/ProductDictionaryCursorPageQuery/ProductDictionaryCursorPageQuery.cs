using Shopway.Domain.Common;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

public sealed record ProductDictionaryCursorPageQuery(CursorPage Page) : ICursorPageQuery<DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    public ProductDictionaryStaticFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
}