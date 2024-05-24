using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Features.Orders.Commands.AddOrderLine;

internal sealed class AddOrderLineCommandValidator : AbstractValidator<AddOrderLineCommand>
{
    public AddOrderLineCommandValidator()
    {
        RuleFor(x => x.Body)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Body.Discount)
                    .MustSatisfyValueObjectValidation(discount => Discount.Validate((decimal)discount!))
                    .When(x => x.Body.Discount is not null);

                RuleFor(x => x.Body.Amount)
                    .MustSatisfyValueObjectValidation(Amount.Validate);
            });
    }
}
