using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Utilities;

namespace Shopway.Application.CQRS.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryHandler : IQueryHandler<GetProductByKeyQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByKeyQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<ProductResponse>> Handle(GetProductByKeyQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetByKeyAsync(query.Key, cancellationToken);

        return product
            .ToResponse()
            .ToResult();
    }
}
