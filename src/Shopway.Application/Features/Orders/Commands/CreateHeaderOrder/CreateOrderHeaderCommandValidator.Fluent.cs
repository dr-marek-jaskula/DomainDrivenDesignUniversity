using FluentValidation;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

internal sealed class CreateOrderHeaderCommandValidator : AbstractValidator<CreateOrderHeaderCommand>
{
    public CreateOrderHeaderCommandValidator()
    {
        RuleFor(x => x.Discount).NotNull();
    }
}