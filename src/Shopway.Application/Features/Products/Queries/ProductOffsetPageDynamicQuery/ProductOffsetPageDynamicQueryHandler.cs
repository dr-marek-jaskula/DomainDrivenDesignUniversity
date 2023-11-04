using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Queries.DynamicOffsetProductQuery;

internal sealed class ProductOffsetPageDynamicQueryHandler : IOffsetPageQueryHandler<ProductOffsetPageDynamicQuery, ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, OffsetPage>
{
    private readonly IProductRepository _productRepository;

    public ProductOffsetPageDynamicQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<OffsetPageResponse<ProductResponse>>> Handle(ProductOffsetPageDynamicQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
