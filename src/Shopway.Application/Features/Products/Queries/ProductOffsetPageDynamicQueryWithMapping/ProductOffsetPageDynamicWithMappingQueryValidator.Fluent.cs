using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;

internal sealed class ProductOffsetPageDynamicWithMappingQueryValidator : OffsetPageQueryValidator<ProductOffsetPageDynamicWithMappingQuery, DataTransferObjectResponse, ProductDynamicFilter, ProductDynamicSortBy, ProductDynamicMapping, OffsetPage>
{
    public ProductOffsetPageDynamicWithMappingQueryValidator()
        : base()
    {
    }
}