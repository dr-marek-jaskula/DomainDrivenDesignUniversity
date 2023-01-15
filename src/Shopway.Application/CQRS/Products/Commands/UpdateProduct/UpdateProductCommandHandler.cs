using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;

    public UpdateProductCommandHandler(IProductRepository productRepository, IValidator validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<IResult<UpdateProductResponse>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Result<Price> priceResult = Price.Create(command.Price);

        _validator
            .Validate(priceResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateProductResponse>();
        }

        var productToUpdate = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        productToUpdate.UpdatePrice(priceResult.Value);

        return productToUpdate
            .ToUpdateResponse()
            .ToResult();
    }
}