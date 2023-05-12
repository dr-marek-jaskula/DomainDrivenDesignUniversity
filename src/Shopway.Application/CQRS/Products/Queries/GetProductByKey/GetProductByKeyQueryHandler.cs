using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using static Shopway.Domain.Errors.HttpErrors;

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
            .GetByKeyOrDefaultAsync(query.Key, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(NotFound<Product>(query.Key));
        }

        return product
            .ToResponse()
            .ToResult();
    }
}
