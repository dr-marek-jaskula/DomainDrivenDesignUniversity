using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Persistence.Framework;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using static Shopway.Domain.Errors.HttpErrors;

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

        _validator
            .If(product is null, thenError: NotFound(nameof(Product), command.ProductId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateReviewResponse>();
        }

        var reviewToUpdate = product!
            .Reviews
            .FirstOrDefault(x => x.Id.Value == command.ReviewId.Value);

        _validator
            .If(reviewToUpdate is null, thenError: NotFound(nameof(Review), command.ReviewId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateReviewResponse>();
        }

        if (command.Description is not null)
        {
            Result<Description> descriptionResult = Description.Create(command.Description);

            _validator
                .Validate(descriptionResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<UpdateReviewResponse>();
            }

            reviewToUpdate!.UpdateDescription(descriptionResult.Value);
        }

        if (command.Stars is not null)
        {
            Result<Stars> starsResult = Stars.Create((decimal)command.Stars);

            _validator
                .Validate(starsResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<UpdateReviewResponse>();
            }

            reviewToUpdate!.UpdateStars(starsResult.Value);
        }

        _reviewRepository.Update(reviewToUpdate!);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = reviewToUpdate!.ToUpdateResponse();

        return Result.Create(response);
    }
}