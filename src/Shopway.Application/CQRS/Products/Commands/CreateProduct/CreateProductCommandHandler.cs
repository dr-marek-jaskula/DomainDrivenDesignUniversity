using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

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

    //There is not need to SaveChanges because is done in the Pipeline (transaction pipeline)
    //Therefore, we return a task, which will slightly increase the performance 
    public Task<IResult<CreateProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Result<ProductName> productNameResult = ProductName.Create(command.ProductName);
        Result<Price> priceResult = Price.Create(command.Price);
        Result<UomCode> uomCodeResult = UomCode.Create(command.UomCode);
        Result<Revision> revisionResult = Revision.Create(command.Revision);

        _validator
            .Validate(productNameResult)
            .Validate(priceResult)
            .Validate(uomCodeResult)
            .Validate(revisionResult);

        if (_validator.IsInvalid)
        {
            var failure = (IResult<CreateProductResponse>)_validator.Failure<CreateProductResponse>();
            return Task.FromResult(failure);
        }

        Product createdProduct = CreateProduct(productNameResult, priceResult, uomCodeResult, revisionResult);

        var response = createdProduct.ToCreateResponse();
        var success = (IResult<CreateProductResponse>)Result.Create(response);

        return Task.FromResult(success);
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