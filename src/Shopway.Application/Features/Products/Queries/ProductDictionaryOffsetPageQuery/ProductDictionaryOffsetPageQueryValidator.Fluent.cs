using Shopway.Domain.Common;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;

internal sealed class ProductDictionaryOffsetPageQueryValidator : OffsetPageQueryValidator<ProductDictionaryOffsetPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDictionaryOffsetPageQueryValidator()
        : base()
    {
    }
}