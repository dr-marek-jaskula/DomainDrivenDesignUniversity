using FluentValidation;

namespace Shopway.Application.Products.Commands.AddReview;

internal sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}