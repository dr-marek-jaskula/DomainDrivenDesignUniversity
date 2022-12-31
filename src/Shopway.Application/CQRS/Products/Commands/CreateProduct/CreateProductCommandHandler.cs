using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator _validator;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IResult<CreateProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
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
            return Result.Failure<CreateProductResponse>(_validator.Error);
        }

        var productId = new ProductId()
        {
            Value = Guid.NewGuid()
        };

        var product = Product.Create(
            productId,
            productNameResult.Value,
            priceResult.Value,
            uomCodeResult.Value,
            revisionResult.Value);

        _productRepository.Create(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateProductResponse(product.Id.Value);
            
        return Result.Create(response);
    }
}