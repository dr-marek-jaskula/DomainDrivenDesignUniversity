using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

internal sealed class RemoveReviewCommandHandler : ICommandHandler<RemoveReviewCommand, RemoveReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveReviewCommandHandler(IProductRepository productRepository, IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<RemoveReviewResponse>> Handle(RemoveReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<RemoveReviewResponse>(HttpErrors.NotFound(nameof(Product), command.ProductId.Value));
        }

        var reviewToRemove = product.Reviews.FirstOrDefault(x => x.Id.Value == command.ReviewId.Value);

        if (reviewToRemove is null)
        {
            return Result.Failure<RemoveReviewResponse>(HttpErrors.NotFound(nameof(Review), command.ReviewId.Value));
        }

        product.RemoveReview(reviewToRemove);

        _reviewRepository.Remove(reviewToRemove);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new RemoveReviewResponse(command.ReviewId.Value);

        return Result.Create(response);
    }
}

