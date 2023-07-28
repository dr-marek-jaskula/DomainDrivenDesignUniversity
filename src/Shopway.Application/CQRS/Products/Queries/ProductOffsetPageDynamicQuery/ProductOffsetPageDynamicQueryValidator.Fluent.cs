using Shopway.Domain.Common;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;

namespace Shopway.Application.CQRS.Products.Queries.DynamicOffsetProductQuery;

internal sealed class ProductOffsetPageDynamicQueryValidator : OffsetPageQueryValidator<ProductOffsetPageDynamicQuery, ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductOffsetPageDynamicQueryValidator()
        : base()
    {
    }
}