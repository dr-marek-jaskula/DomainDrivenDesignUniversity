using FluentValidation;

namespace Shopway.Application.Features.Products.Commands.AddReview;

internal sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Body).NotNull();
        RuleFor(x => x.Body.Title).NotNull();
        RuleFor(x => x.Body.Stars).NotNull();
        RuleFor(x => x.Body.Description).NotNull();
    }
}