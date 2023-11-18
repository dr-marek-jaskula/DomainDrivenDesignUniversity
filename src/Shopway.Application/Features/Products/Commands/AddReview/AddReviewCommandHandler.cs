using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Abstractions;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Commands.AddReview;

internal sealed class AddReviewCommandHandler(IProductRepository productRepository, IValidator validator, IUserContextService userContext) 
    : ICommandHandler<AddReviewCommand, AddReviewResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator _validator = validator;
    private readonly IUserContextService _userContext = userContext;

    public async Task<IResult<AddReviewResponse>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Title> titleResult = Title.Create(command.Body.Title);
        ValidationResult<Description> descriptionResult = Description.Create(command.Body.Description);
        ValidationResult<Username> usernameResult = Username.Create(_userContext.Username!);
        ValidationResult<Stars> starsResult = Stars.Create(command.Body.Stars);

        _validator
            .Validate(titleResult)
            .Validate(descriptionResult)
            .Validate(usernameResult)
            .Validate(starsResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AddReviewResponse>();
        }

        var product = await _productRepository.GetByIdWithReviewAsync(command.ProductId, titleResult.Value, cancellationToken);

        _validator
            .If(product.Reviews.Any(), thenError: Error.AlreadyExists(ReviewKey.Create(titleResult.Value.Value)));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AddReviewResponse>();
        }

        Review reviewToAdd = CreateReview(titleResult.Value, descriptionResult.Value, usernameResult.Value, starsResult.Value);

        product.AddReview(reviewToAdd);

        return reviewToAdd
            .ToAddResponse()
            .ToResult();
    }

    private static Review CreateReview(Title title, Description description, Username username, Stars stars)
    {
        return Review.Create
        (
            ReviewId.New(),
            title,
            description,
            username,
            stars
        );
    }
}
