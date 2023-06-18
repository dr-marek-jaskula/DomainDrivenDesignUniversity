using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Specifications.Products.Filtering;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Application.Utilities;
using Shopway.Domain.Common;

namespace Shopway.Application.CQRS.Products.Queries.QueryProductByExpression;

internal sealed class ProductPageExpressionQueryHandler : IPageQueryHandler<ProductPageExpressionQuery, ProductResponse, ProductExpressionFilter, ProductOrder, Page>
{
    private readonly IProductRepository _productRepository;

    public ProductPageExpressionQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<ProductResponse>>> Handle(ProductPageExpressionQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, pageQuery.Filter, pageQuery.Order, ProductMapping.ProductResponse, cancellationToken);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
