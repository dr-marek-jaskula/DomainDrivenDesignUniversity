using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.ProductCursorPageDynamicWithMappingQuery;

internal sealed class ProductCursorPageDynamicWithMappingQueryValidator : CursorPageQueryValidator<ProductCursorPageDynamicWithMappingQuery, DataTransferObjectResponse, ProductDynamicFilter, ProductDynamicSortBy, ProductDynamicMapping, CursorPage>
{
    public ProductCursorPageDynamicWithMappingQueryValidator()
        : base()
    {
    }
}
