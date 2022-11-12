using FluentValidation;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Products.Commands.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ReviewId).NotEmpty();

        RuleFor(x => new { x.Stars, x.Description })
            .Must(x => x.Stars is not null || x.Description is not null)
            .WithMessage($"{nameof(Stars)} and {nameof(Description)} can be both null");
    }
}