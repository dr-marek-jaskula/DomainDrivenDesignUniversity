using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryHandler : IPageQueryHandler<ProductPageQuery, ProductResponse, ProductStaticFilter, ProductStaticSortBy, Page>
{
    private readonly IProductRepository _productRepository;

    public ProductPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<ProductResponse>>> Handle(ProductPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, staticFilter: pageQuery.Filter, staticSort: pageQuery.SortBy, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
