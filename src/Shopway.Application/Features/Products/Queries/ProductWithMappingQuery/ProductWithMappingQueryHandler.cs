using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Queries.DynamicProductWithMappingQuery;

internal sealed class ProductWithMappingQueryHandler(IProductRepository productRepository)
    : IQueryHandler<ProductWithMappingQuery, DataTransferObjectResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<DataTransferObjectResponse>> Handle(ProductWithMappingQuery query, CancellationToken cancellationToken)
    {
        var productDto = await _productRepository
            .QueryByIdAsync(query.ProductId, cancellationToken, query.Mapping);

        return productDto
            .ToResponse()
            .ToResult();
    }
}
