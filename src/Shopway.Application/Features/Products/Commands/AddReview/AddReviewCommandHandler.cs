using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.AddReview;

internal sealed class AddReviewCommandHandler(IProductRepository productRepository, IValidator validator, IUserContextService userContext)
    : ICommandHandler<AddReviewCommand, AddReviewResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator _validator = validator;
    private readonly IUserContextService _userContext = userContext;

    public async Task<IResult<AddReviewResponse>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        var title = Title.Create(command.Body.Title).Value;

        var product = await _productRepository.GetByIdWithReviewAsync(command.ProductId, title, cancellationToken);

        _validator
            .If(product.Reviews.Count is not 0, thenError: Error.AlreadyExists(ReviewKey.Create(title.Value)));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AddReviewResponse>();
        }

        Description description = Description.Create(command.Body.Description).Value;
        Username username = Username.Create(_userContext.Username!).Value;
        Stars stars = Stars.Create(command.Body.Stars).Value;

        Review reviewToAdd = CreateReview(title, description, username, stars);

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
