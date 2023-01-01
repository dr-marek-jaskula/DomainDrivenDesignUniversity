using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, UpdateReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator _validator;

    public UpdateReviewCommandHandler(IProductRepository productRepository, IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _validator=validator;
    }

    public async Task<IResult<UpdateReviewResponse>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<UpdateReviewResponse>(HttpErrors.NotFound(nameof(Product), command.ProductId.Value));
        }

        var reviewToUpdate = product.Reviews.FirstOrDefault(x => x.Id.Value == command.ReviewId.Value);

        if (reviewToUpdate is null)
        {
            return Result.Failure<UpdateReviewResponse>(HttpErrors.NotFound(nameof(Review), command.ReviewId.Value));
        }

        Error error = Error.None;

        if (command.Description is not null)
        {
            Result<Description> descriptionResult = Description.Create(command.Description);

            _validator.Validate(descriptionResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<UpdateReviewResponse>();
            }

            reviewToUpdate.UpdateDescription(descriptionResult.Value);
        }

        if (command.Stars is not null)
        {
            Result<Stars> starsResult = Stars.Create((decimal)command.Stars);

            _validator.Validate(starsResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<UpdateReviewResponse>();
            }

            reviewToUpdate.UpdateStars(starsResult.Value);
        }

        _reviewRepository.Update(reviewToUpdate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new UpdateReviewResponse(reviewToUpdate.Id.Value);

        return Result.Create(response);
    }
}