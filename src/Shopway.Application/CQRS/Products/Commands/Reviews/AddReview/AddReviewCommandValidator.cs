using FluentValidation;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;

internal sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}