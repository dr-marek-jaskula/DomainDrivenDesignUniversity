using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.AddReview;

internal sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Body)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Body.Title)
                    .MustSatisfyValueObjectValidation(Title.Validate);

                RuleFor(x => x.Body.Stars)
                    .MustSatisfyValueObjectValidation(Stars.Validate);

                RuleFor(x => x.Body.Description)
                    .MustSatisfyValueObjectValidation(Description.Validate);
            });
    }
}
