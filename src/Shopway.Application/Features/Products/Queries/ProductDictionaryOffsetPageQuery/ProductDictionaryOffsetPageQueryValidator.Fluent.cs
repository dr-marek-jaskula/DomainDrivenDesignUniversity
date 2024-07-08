using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;

internal sealed class ProductDictionaryOffsetPageQueryValidator 
    : OffsetPageQueryValidator<ProductDictionaryOffsetPageQuery, DictionaryResponseEntry<ProductKey>, ProductDictionaryStaticFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductDictionaryOffsetPageQueryValidator()
        : base()
    {
    }
}
