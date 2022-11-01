using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(new Error(
                "Product.NotFound",
                $"The product with Id: {request.ProductId} was not found"));
        }

        var response = new ProductResponse(
            Id: product.Id,
            ProductName: product.ProductName,
            Revision: product.Revision,
            Price: product.Price,
            UomCode: product.UomCode,
            Reviews: product.Reviews);

        return response;
    }
}

