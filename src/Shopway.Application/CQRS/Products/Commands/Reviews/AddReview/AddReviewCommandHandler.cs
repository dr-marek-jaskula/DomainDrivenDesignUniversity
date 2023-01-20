using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;

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

        Result<Title> titleResult = Title.Create(command.Title);
        Result<Description> descriptionResult = Description.Create(command.Description);
        Result<Username> usernameResult = Username.Create(_userContext.GetUserName!);
        Result<Stars> starsResult = Stars.Create(command.Stars);

        _validator
            .Validate(titleResult)
            .Validate(descriptionResult)
            .Validate(usernameResult)
            .Validate(starsResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AddReviewResponse>();
        }

        var reviewToAdd = product
            .AddReview(titleResult.Value, descriptionResult.Value, usernameResult.Value, starsResult.Value);

        return reviewToAdd
            .ToAddResponse()
            .ToResult();
    }
}
