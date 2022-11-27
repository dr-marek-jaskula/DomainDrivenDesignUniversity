using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.Products.Commands.RemoveReview;

internal sealed class RemoveReviewCommandHandler : ICommandHandler<RemoveReviewCommand, Guid>
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

    public async Task<Result<Guid>> Handle(RemoveReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<Guid>(HttpErrors.NotFound(nameof(Product), command.ProductId.Value));
        }

        var reviewToRemove = product.Reviews.FirstOrDefault(x => x.Id.Value == command.ReviewId.Value);

        if (reviewToRemove is null)
        {
            return Result.Failure<Guid>(HttpErrors.NotFound(nameof(Review), command.ReviewId.Value));
        }

        product.RemoveReview(reviewToRemove);

        _reviewRepository.Remove(reviewToRemove);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return command.ReviewId.Value;
     }
}

