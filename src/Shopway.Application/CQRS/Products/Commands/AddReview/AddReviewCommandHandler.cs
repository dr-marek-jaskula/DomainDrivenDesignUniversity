using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.EntityBusinessKeys;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Products.Commands.AddReview;

internal sealed class AddReviewCommandHandler : ICommandHandler<AddReviewCommand, AddReviewResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator _validator;
    private readonly IUserContextService _userContext;

    public AddReviewCommandHandler(IProductRepository productRepository, IValidator validator, IUserContextService userContext)
    {
        _productRepository = productRepository;
        _validator = validator;
        _userContext = userContext;
    }

    public async Task<IResult<AddReviewResponse>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

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

        _validator
            .If(product.AnyReview(titleResult.Value), thenError: AlreadyExists(ReviewKey.Create(product.ToProductKey(), titleResult.Value.Value)));

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
