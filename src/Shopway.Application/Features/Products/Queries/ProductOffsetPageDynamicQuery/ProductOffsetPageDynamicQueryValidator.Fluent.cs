using Shopway.Domain.Common;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;

internal sealed class ProductOffsetPageDynamicQueryValidator : OffsetPageQueryValidator<ProductOffsetPageDynamicQuery, ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, OffsetPage>
{
    public ProductOffsetPageDynamicQueryValidator()
        : base()
    {
    }
}