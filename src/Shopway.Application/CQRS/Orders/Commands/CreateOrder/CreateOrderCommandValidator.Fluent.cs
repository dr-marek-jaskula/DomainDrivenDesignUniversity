using FluentValidation;

namespace Shopway.Application.CQRS.Orders.Commands.CreateOrder;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Amount).NotNull().NotEmpty().GreaterThan(0);
        RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        RuleFor(x => x.Discount).NotNull();
    }
}

