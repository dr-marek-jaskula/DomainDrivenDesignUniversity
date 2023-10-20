using Shopway.Domain.Common;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

internal sealed class ProductDictionaryCursorPageQueryValidator : CursorPageQueryValidator<ProductDictionaryCursorPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    public ProductDictionaryCursorPageQueryValidator()
        : base()
    {
    }
}