using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.UpdateReview;

internal sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.ReviewId)
            .NotEmpty();

        RuleFor(x => new { x.Body.Stars, x.Body.Description })
            .Must(x => x.Stars is not null || x.Description is not null)
            .WithMessage($"{nameof(Stars)} and {nameof(Description)} can be both null")
            .DependentRules(() =>
            {
                When(x => x.Body.Stars is not null, () =>
                {
                    RuleFor(x => x.Body.Stars)
                        .MustSatisfyValueObjectValidation(stars => Stars.Validate((decimal)stars!));
                });

                When(x => x.Body.Description is not null, () =>
                {
                    RuleFor(x => x.Body.Description)
                        .MustSatisfyValueObjectValidation(description => Description.Validate(description!));
                });
            });
    }
}
