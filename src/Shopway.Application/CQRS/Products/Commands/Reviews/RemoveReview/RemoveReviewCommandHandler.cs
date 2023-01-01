using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

internal sealed class RemoveReviewCommandHandler : ICommandHandler<RemoveReviewCommand, RemoveReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator _validator;

    public RemoveReviewCommandHandler(IProductRepository productRepository, IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IValidator validator)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IResult<RemoveReviewResponse>> Handle(RemoveReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        _validator
            .If(product is null, thenError: HttpErrors.NotFound(nameof(Product), command.ProductId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RemoveReviewResponse>();
        }

        var reviewToRemove = product!.Reviews.FirstOrDefault(x => x.Id.Value == command.ReviewId.Value);

        _validator
            .If(reviewToRemove is null, thenError: HttpErrors.NotFound(nameof(Review), command.ReviewId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RemoveReviewResponse>();
        }

        product.RemoveReview(reviewToRemove!);

        _reviewRepository.Remove(reviewToRemove!);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = reviewToRemove!.ToRemoveResponse();

        return Result.Create(response);
    }
}

