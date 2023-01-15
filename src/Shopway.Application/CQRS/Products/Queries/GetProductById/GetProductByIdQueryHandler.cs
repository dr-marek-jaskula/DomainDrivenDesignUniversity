using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Results;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;

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

        var response = product!.ToResponse();

        return Result.Create(response);
    }
}
