using Shopway.Domain.Common;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;

internal sealed class ProductOffsetPageQueryValidator : OffsetPageQueryValidator<ProductOffsetPageQuery, ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    public ProductOffsetPageQueryValidator()
        : base()
    {
    }
}