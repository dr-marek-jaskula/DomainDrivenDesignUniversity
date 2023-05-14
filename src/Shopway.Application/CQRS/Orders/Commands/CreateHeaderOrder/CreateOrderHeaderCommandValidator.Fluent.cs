using FluentValidation;

namespace Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;

internal sealed class CreateOrderHeaderCommandValidator : AbstractValidator<CreateOrderHeaderCommand>
{
    public CreateOrderHeaderCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty();
        RuleFor(x => x.Discount).NotNull();
    }
}