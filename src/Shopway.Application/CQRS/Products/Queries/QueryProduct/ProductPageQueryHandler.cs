using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryHandler : IPageQueryHandler<ProductPageQuery, ProductResponse, ProductFilter, ProductOrder, Page>
{
    private readonly IProductRepository _productRepository;

    public ProductPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<ProductResponse>>> Handle(ProductPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, pageQuery.Filter, pageQuery.Order, ProductMapping.ToResponse(), cancellationToken);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
