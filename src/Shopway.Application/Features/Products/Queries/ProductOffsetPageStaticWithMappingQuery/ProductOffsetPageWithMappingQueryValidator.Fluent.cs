using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProductWithMapping;

internal sealed class ProductOffsetPageWithMappingQueryValidator : OffsetPageQueryValidator<ProductOffsetPageWithMappingQuery, DataTransferObjectResponse, ProductStaticFilter, ProductStaticSortBy, ProductStaticMapping, OffsetPage>
{
    public ProductOffsetPageWithMappingQueryValidator()
        : base()
    {
    }
}
