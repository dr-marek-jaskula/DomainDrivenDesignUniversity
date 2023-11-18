using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler(IProductRepository productRepository, IValidator validator) 
    : ICommandHandler<UpdateReviewCommand, UpdateReviewResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator _validator = validator;

    //It is not preferred to make partial updates, but for tutorial purpose it is done here
    public async Task<IResult<UpdateReviewResponse>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithReviewAsync(command.ProductId, command.ReviewId, cancellationToken);

        var reviewToUpdate = product
            .Reviews
            .First(x => x.Id == command.ReviewId);

        ValidationResult<Description>? descriptionResult = null;
        ValidationResult<Stars>? starsResult = null;

        if (command.Body.Description is not null)
        {
            descriptionResult = Description.Create(command.Body.Description);
            _validator.Validate(descriptionResult);
        }

        if (command.Body.Stars is not null)
        {
            starsResult = Stars.Create((decimal)command.Body.Stars);
            _validator.Validate(starsResult);
        }

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UpdateReviewResponse>();
        }

        Update(reviewToUpdate, descriptionResult, starsResult);

        return reviewToUpdate
            .ToUpdateResponse()
            .ToResult();
    }

    private static void Update(Review reviewToUpdate, ValidationResult<Description>? description, ValidationResult<Stars>? starts)
    {
        if (description is not null)
        {
            reviewToUpdate.UpdateDescription(description.Value);
        }

        if (starts is not null)
        {
            reviewToUpdate.UpdateStars(starts.Value);
        }
    }
}