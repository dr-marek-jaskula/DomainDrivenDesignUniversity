using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Products.Commands.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(IProductRepository productRepository, IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<Guid>(HttpErrors.NotFound(nameof(Product), command.ProductId.Value));
        }

        var reviewToUpdate = product.Reviews.FirstOrDefault(x => x.Id.Value == command.ReviewId.Value);

        if (reviewToUpdate is null)
        {
            return Result.Failure<Guid>(HttpErrors.NotFound(nameof(Review), command.ReviewId.Value));
        }

        Error error = Error.None;

        if (command.Description is not null)
        {
            Result<Description> descriptionResult = Description.Create(command.Description);
            error = ErrorHandler.FirstValueObjectErrorOrErrorNone(descriptionResult);

            if (error != Error.None)
            {
                return Result.Failure<Guid>(error);
            }

            reviewToUpdate.UpdateDescription(descriptionResult.Value);
        }

        if (command.Stars is not null)
        {
            Result<Stars> starsResult = Stars.Create((decimal)command.Stars);
            error = ErrorHandler.FirstValueObjectErrorOrErrorNone(starsResult);

            if (error != Error.None)
            {
                return Result.Failure<Guid>(error);
            }

            reviewToUpdate.UpdateStars(starsResult.Value);
        }

        _reviewRepository.Update(reviewToUpdate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return reviewToUpdate.Id.Value;
    }
}