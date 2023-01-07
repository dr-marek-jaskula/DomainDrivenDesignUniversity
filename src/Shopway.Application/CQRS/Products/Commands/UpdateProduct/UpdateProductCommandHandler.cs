using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator _validator;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IResult<UpdateProductResponse>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Result<Price> priceResult = Price.Create(command.Price);

        _validator.Validate(priceResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateProductResponse>();
        }

        var productToUpdate = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        _validator
            .If(productToUpdate is null, thenError: NotFound(nameof(Product), command.Id));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateProductResponse>();
        }

        productToUpdate!.UpdatePrice(priceResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = productToUpdate.ToUpdateResponse();

        return Result.Create(response);
    }
}