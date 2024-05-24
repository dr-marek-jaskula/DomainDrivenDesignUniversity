using FluentValidation;
using Shopway.Application.Utilities;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class CreateOrderHeaderCommandValidator : AbstractValidator<CreateOrderHeaderCommand>
{
    public CreateOrderHeaderCommandValidator()
    {
        RuleFor(x => x.Discount)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Discount)
                    .MustSatisfyValueObjectValidation(discount => Discount.Validate((decimal)discount!));
            });
    }
}
