using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Sorting.Products;
using Shopway.Application.Filering.Products;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;

internal sealed class ProductOffsetPageQueryHandler(IProductRepository productRepository)
    : IOffsetPageQueryHandler<ProductOffsetPageQuery, ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<OffsetPageResponse<ProductResponse>>> Handle(ProductOffsetPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
