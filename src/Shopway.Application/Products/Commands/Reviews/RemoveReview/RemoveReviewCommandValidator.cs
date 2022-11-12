using FluentValidation;
using Shopway.Application.Products.Commands.RemoveReview;

namespace Shopway.Application.Products.Commands.AddReview;

internal sealed class RemoveReviewCommandValidator : AbstractValidator<RemoveReviewCommand>
{
    public RemoveReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ReviewId).NotEmpty();
    }
}