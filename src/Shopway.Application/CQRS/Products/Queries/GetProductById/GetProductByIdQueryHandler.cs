using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;

namespace Shopway.Application.CQRS.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IValidator validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<IResult<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, cancellationToken);

        _validator
            .If(product is null, thenError: HttpErrors.NotFound(nameof(Product), query.Id));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<ProductResponse>();
        }

        var response = new ProductResponse(
            Id: product!.Id.Value,
            ProductName: product!.ProductName,
            Revision: product!.Revision,
            Price: product!.Price,
            UomCode: product!.UomCode,
            Reviews: product!.Reviews);

        return Result.Create(response);
    }
}

