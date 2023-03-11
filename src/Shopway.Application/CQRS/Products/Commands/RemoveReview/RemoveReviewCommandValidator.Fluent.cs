using FluentValidation;

namespace Shopway.Application.CQRS.Products.Commands.RemoveReview;

internal sealed class RemoveReviewCommandValidator : AbstractValidator<RemoveReviewCommand>
{
    public RemoveReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ReviewId).NotEmpty();
    }
}