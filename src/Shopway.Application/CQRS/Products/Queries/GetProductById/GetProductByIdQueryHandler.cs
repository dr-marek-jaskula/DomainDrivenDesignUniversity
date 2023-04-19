using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Utilities;

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
        var product = await _productRepository
            .GetByIdAsync(query.Id, cancellationToken);

        return product
            .ToResponse()
            .ToResult();
    }
}
