using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.CQRS.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(HttpErrors.NotFound(nameof(Product), query.Id.Value));
        }

        var response = new ProductResponse(
            Id: product.Id.Value,
            ProductName: product.ProductName,
            Revision: product.Revision,
            Price: product.Price,
            UomCode: product.UomCode,
            Reviews: product.Reviews);

        return Result.Create(response);
    }
}

