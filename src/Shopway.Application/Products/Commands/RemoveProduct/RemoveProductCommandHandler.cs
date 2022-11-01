using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandHandler : ICommandHandler<RemoveProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RemoveProductCommand command, CancellationToken cancellationToken)
    {
        //Data to create product that will be attached to database context
        Result<ProductName> productNameResult = ProductName.Create("ToDelete");
        Result<Price> priceResult = Price.Create(1m);
        Result<UomCode> uomCodeResult = UomCode.Create("kg");
        Result<Revision> revisionResult = Revision.Create("0");

        Error error = ErrorHandler.FirstValueObjectErrorOrErrorNone(productNameResult, priceResult, uomCodeResult, revisionResult);

        if (error != Error.None)
        {
            return Result.Failure<Guid>(new(error.Code, error.Message));
        }

        var productToDelete = Product.Create(
            command.Id,
            productNameResult.Value,
            priceResult.Value,
            uomCodeResult.Value,
            revisionResult.Value);

        _productRepository.Remove(productToDelete);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return productToDelete.Id;
        }
        catch
        {
            return Result.Failure<Guid>(HttpErrors.NotFound(nameof(Product), command.Id));
        }
    }
}