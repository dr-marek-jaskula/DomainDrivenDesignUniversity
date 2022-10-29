using FluentValidation;

namespace Shopway.Application.Orders.Commands.CreateOrder;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);

        RuleFor(x => x.CustomerId).NotEmpty();
    }
}

