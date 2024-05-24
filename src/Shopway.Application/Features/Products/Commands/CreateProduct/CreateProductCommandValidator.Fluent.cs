using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Price)
            .MustSatisfyValueObjectValidation(Price.Validate);

        RuleFor(x => x.UomCode)
            .MustSatisfyValueObjectValidation(UomCode.Validate);

        RuleFor(x => x.ProductKey)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.ProductKey.ProductName)
                    .MustSatisfyValueObjectValidation(ProductName.Validate);

                RuleFor(x => x.ProductKey.Revision)
                    .MustSatisfyValueObjectValidation(Revision.Validate);
            });
    }
}
