using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Queries.GetProductByKey;

internal sealed class GetProductByKeyQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductByKeyQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<ProductResponse>> Handle(GetProductByKeyQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetByKeyOrDefaultAsync(query.Key, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(Error.NotFound<Product>(query.Key));
        }

        return product
            .ToResponse()
            .ToResult();
    }
}
