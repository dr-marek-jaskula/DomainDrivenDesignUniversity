using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, UpdateReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;

    public UpdateReviewCommandHandler(IProductRepository productRepository, IValidator validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<IResult<UpdateReviewResponse>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        var reviewToUpdate = product
            .Reviews
            .First(x => x.Id == command.ReviewId);

        if (command.Body.Description is not null)
        {
            ValidationResult<Description> descriptionResult = Description.Create(command.Body.Description);

            _validator
                .Validate(descriptionResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<UpdateReviewResponse>();
            }

            reviewToUpdate.UpdateDescription(descriptionResult.Value);
        }

        if (command.Body.Stars is not null)
        {
            ValidationResult<Stars> starsResult = Stars.Create((decimal)command.Body.Stars);

            _validator
                .Validate(starsResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<UpdateReviewResponse>();
            }

            reviewToUpdate.UpdateStars(starsResult.Value);
        }

        return reviewToUpdate
            .ToUpdateResponse()
            .ToResult();
    }
}