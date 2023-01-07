using MediatR;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandHandler : ICommandHandler<RemoveProductCommand, RemoveProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator _validator;

    public RemoveProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IResult<RemoveProductResponse>> Handle(RemoveProductCommand command, CancellationToken cancellationToken)
    {
        //Data to create product that will be attached to database context
        Result<ProductName> productNameResult = ProductName.Create("ToDelete");
        Result<Price> priceResult = Price.Create(1m);
        Result<UomCode> uomCodeResult = UomCode.Create("kg");
        Result<Revision> revisionResult = Revision.Create("0");

        _validator
            .Validate(productNameResult)
            .Validate(priceResult)
            .Validate(uomCodeResult)
            .Validate(revisionResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RemoveProductResponse>();
        }

        var productToRemove = Product.Create(
            command.Id,
            productNameResult.Value,
            priceResult.Value,
            uomCodeResult.Value,
            revisionResult.Value);

        _productRepository.Remove(productToRemove);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var response = productToRemove.ToRemoveResponse();
            return Result.Create(response);
        }
        catch
        {
            return Result.Failure<RemoveProductResponse>(NotFound(nameof(Product), command.Id));
        }
    }
}