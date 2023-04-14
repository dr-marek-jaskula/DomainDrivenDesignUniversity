using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;
using Shopway.Domain.Entities;

namespace Shopway.Application.CQRS.Products.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, UpdateReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;

    public UpdateReviewCommandHandler(IProductRepository productRepository, IValidator validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    //It is not preferred to make partial updates, but for tutorial purpose it is done here
    public async Task<IResult<UpdateReviewResponse>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

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