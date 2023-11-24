using Shopway.Domain.Products;
using Shopway.Application.Mappings;
using Shopway.Domain.Common.Results;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(IProductRepository productRepository, IValidator validator)
    : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<UpdateProductResponse>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Price> priceResult = Price.Create(command.Body.Price);

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