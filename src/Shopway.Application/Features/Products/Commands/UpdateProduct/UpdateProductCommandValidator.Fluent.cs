using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotNull();

        RuleFor(x => x.Body)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Body.Price)
                    .MustSatisfyValueObjectValidation(Price.Validate);
            });
    }
}
