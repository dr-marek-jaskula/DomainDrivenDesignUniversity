using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;

internal sealed class AddReviewCommandHandler : ICommandHandler<AddReviewCommand, AddReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator _validator;

    public AddReviewCommandHandler(IProductRepository productRepository, IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IResult<AddReviewResponse>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        _validator
            .If(product is null, HttpErrors.NotFound(nameof(Product), command.ProductId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AddReviewResponse>();
        }

        Result<Title> titleResult = Title.Create(command.Title);
        Result<Description> descriptionResult = Description.Create(command.Description);
        Result<Username> usernameResult = Username.Create(command.Username);
        Result<Stars> starsResult = Stars.Create(command.Stars);

        _validator
            .Validate(titleResult)
            .Validate(descriptionResult)
            .Validate(usernameResult)
            .Validate(starsResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AddReviewResponse>();
        }

        var reviewAdded = product!.AddReview(titleResult.Value, descriptionResult.Value, usernameResult.Value, starsResult.Value);

        _reviewRepository.Add(reviewAdded);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AddReviewResponse(reviewAdded.Id.Value);

        return Result.Create(response);
    }
}
