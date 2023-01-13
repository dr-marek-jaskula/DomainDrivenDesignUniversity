using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Application.Mapping;
using Shopway.Application.Utilities;
using Shopway.Persistence.Specifications.Products;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;

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
        var queryable = Queryable(pageQuery);

        var pageResponse = await queryable
            .ToPage(pageQuery.PageSize, pageQuery.PageNumber, ToResponse, cancellationToken);

        return Result.Create(pageResponse);
    }

    private IQueryable<Product> Queryable(ProductPageQuery pageQuery)
    {
        return _productRepository
            .Queryable(pageQuery.Filter, pageQuery.Order);
    }

    private static ProductResponse ToResponse(Product entitiy)
    {
        return ProductMapping.ToResponse(entitiy);
    }
}
