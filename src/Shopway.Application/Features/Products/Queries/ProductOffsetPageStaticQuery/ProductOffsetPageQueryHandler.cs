using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProduct;

internal sealed class ProductOffsetPageQueryHandler : IOffsetPageQueryHandler<ProductOffsetPageQuery, ProductResponse, ProductStaticFilter, ProductStaticSortBy, OffsetPage>
{
    private readonly IProductRepository _productRepository;

    public ProductOffsetPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<OffsetPageResponse<ProductResponse>>> Handle(ProductOffsetPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, staticFilter: pageQuery.Filter, staticSort: pageQuery.SortBy, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
