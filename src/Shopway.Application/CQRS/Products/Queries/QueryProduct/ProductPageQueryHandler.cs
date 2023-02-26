using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Persistence.Specifications.Products;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mapping;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

internal sealed class ProductPageQueryHandler : IPageQueryHandler<ProductPageQuery, ProductResponse, ProductFilter, ProductOrder>
{
    private readonly IProductRepository _productRepository;

    public ProductPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<ProductResponse>>> Handle(ProductPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var queryable = _productRepository
            .Queryable(pageQuery.Filter, pageQuery.Order);

        var pageResponse = await queryable
            .ToPageResponse(pageQuery.PageSize, pageQuery.PageNumber, ProductMapping.ToResponse, cancellationToken);

        return pageResponse
            .ToResult();
    }
}
