using FluentValidation;

namespace Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

internal sealed class ChangeOrderHeaderStatusCommandValidator : AbstractValidator<ChangeOrderHeaderStatusCommand>
{
    public ChangeOrderHeaderStatusCommandValidator()
    {
        RuleFor(command => command.OrderHeaderId).NotEmpty();
        RuleFor(command => command.Body.NewOrderHeaderStatus).NotEmpty();
    }
}