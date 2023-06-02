using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityKeys;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;

    public CreateProductCommandHandler(IProductRepository productRepository, IValidator validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<IResult<CreateProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<ProductName> productNameResult = ProductName.Create(command.ProductKey.ProductName);
        ValidationResult<Revision> revisionResult = Revision.Create(command.ProductKey.Revision);
        ValidationResult<Price> priceResult = Price.Create(command.Price);
        ValidationResult<UomCode> uomCodeResult = UomCode.Create(command.UomCode);

        _validator
            .Validate(productNameResult)
            .Validate(priceResult)
            .Validate(uomCodeResult)
            .Validate(revisionResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<CreateProductResponse>();
        }

        _validator
            .If(await ProductAlreadyExists(productNameResult.Value, revisionResult.Value, cancellationToken), AlreadyExists(command.ProductKey));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<CreateProductResponse>();
        }

        Product createdProduct = CreateProduct(productNameResult, priceResult, uomCodeResult, revisionResult);

        return createdProduct
            .ToCreateResponse()
            .ToResult();
    }

    private async Task<bool> ProductAlreadyExists(ProductName productName, Revision revision, CancellationToken cancellationToken)
    {
        return await _productRepository
            .AnyAsync(productName, revision, cancellationToken);
    }

    private Product CreateProduct
    (
        Result<ProductName> productNameResult,
        Result<Price> priceResult,
        Result<UomCode> uomCodeResult,
        Result<Revision> revisionResult
    )
    {
        var productToCreate = Product.Create
        (
            id: ProductId.New(),
            productName: productNameResult.Value,
            price: priceResult.Value,
            uomCode: uomCodeResult.Value,
            revision: revisionResult.Value
        );

        _productRepository.Create(productToCreate);

        return productToCreate;
    }
}