using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.CreateProduct;

//Option without using my own IValidator (for tutorial purposes). Due to the fact the most of the validation is moved to Fluent Validation
//and that this approach does not create an Error instance if not needed (and IValidator does - extra cost), this approach may be 
//preferable. I leave it to viewer decision.
internal sealed class CreateProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<CreateProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        if (await ProductAlreadyExists(command.ProductKey, cancellationToken))
        {
            return Error.AlreadyExists(command.ProductKey)
                .ToValidationResult<CreateProductResponse>();
        }

        var productName = ProductName.Create(command.ProductKey.ProductName).Value;
        var revision = Revision.Create(command.ProductKey.Revision).Value;
        var price = Price.Create(command.Price).Value;
        var uomCode = UomCode.Create(command.UomCode).Value;

        Product createdProduct = CreateProduct(productName, price, uomCode, revision);

        return createdProduct
            .ToCreateResponse()
            .ToResult();
    }

    private async Task<bool> ProductAlreadyExists(ProductKey productKey, CancellationToken cancellationToken)
    {
        return await _productRepository
            .AnyAsync(productKey, cancellationToken);
    }

    private Product CreateProduct
    (
        ProductName productName,
        Price price,
        UomCode uomCode,
        Revision revision
    )
    {
        var productToCreate = Product.Create
        (
            id: ProductId.New(),
            productName: productName,
            price: price,
            uomCode: uomCode,
            revision: revision
        );

        _productRepository.Create(productToCreate);

        return productToCreate;
    }
}
