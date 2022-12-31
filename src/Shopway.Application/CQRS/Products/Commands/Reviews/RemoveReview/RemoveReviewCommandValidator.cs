using FluentValidation;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

internal sealed class RemoveReviewCommandValidator : AbstractValidator<RemoveReviewCommand>
{
    public RemoveReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ReviewId).NotEmpty();
    }
}