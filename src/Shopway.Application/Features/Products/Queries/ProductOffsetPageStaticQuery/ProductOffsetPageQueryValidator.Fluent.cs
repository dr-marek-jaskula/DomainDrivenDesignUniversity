using Shopway.Domain.Common;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;

internal sealed class ProductOffsetPageQueryValidator : OffsetPageQueryValidator<ProductOffsetPageQuery, ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    public ProductOffsetPageQueryValidator()
        : base()
    {
    }
}