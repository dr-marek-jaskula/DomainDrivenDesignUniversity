using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<UpdateProductResponse>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var price = Price.Create(command.Body.Price).Value;

        var productToUpdate = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        productToUpdate.UpdatePrice(price);

        return productToUpdate
            .ToUpdateResponse()
            .ToResult();
    }
}
