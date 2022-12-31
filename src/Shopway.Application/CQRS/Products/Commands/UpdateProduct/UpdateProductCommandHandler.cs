using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<UpdateProductResponse>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Result<Price> priceResult = Price.Create(command.Price);

        Error error = ErrorHandler.FirstValueObjectErrorOrErrorNone(priceResult);

        if (error != Error.None)
        {
            return Result.Failure<UpdateProductResponse>(error);
        }

        var productToUpdate = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        if (productToUpdate is null)
        {
            return Result.Failure<UpdateProductResponse>(HttpErrors.NotFound(nameof(Product), command.Id.Value));
        }

        productToUpdate.UpdatePrice(priceResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new UpdateProductResponse(productToUpdate.Id.Value);

        return Result.Create(response);
    }
}