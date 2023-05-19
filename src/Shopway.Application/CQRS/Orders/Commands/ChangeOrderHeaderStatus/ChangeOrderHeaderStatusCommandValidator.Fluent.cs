using FluentValidation;

namespace Shopway.Application.CQRS.Orders.Commands.ChangeOrderHeaderStatus;

internal sealed class ChangeOrderHeaderStatusCommandValidator : AbstractValidator<ChangeOrderHeaderStatusCommand>
{
    public ChangeOrderHeaderStatusCommandValidator()
    {
        RuleFor(command => command.OrderHeaderId).NotEmpty();
        RuleFor(command => command.Body.NewOrderHeaderStatus).NotEmpty();
    }
}