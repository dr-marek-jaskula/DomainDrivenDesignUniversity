using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Application.Utilities;
using Shopway.Domain.Common;

namespace Shopway.Application.CQRS.Products.Queries.QueryProductByExpression;

internal sealed class ProductPageDynamicQueryHandler : IPageQueryHandler<ProductPageDynamicQuery, ProductResponse, ProductDynamicFilter, ProductDynamicSortBy, Page>
{
    private readonly IProductRepository _productRepository;

    public ProductPageDynamicQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<ProductResponse>>> Handle(ProductPageDynamicQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, dynamicFilter: pageQuery.Filter, dynamicSort: pageQuery.SortBy, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
