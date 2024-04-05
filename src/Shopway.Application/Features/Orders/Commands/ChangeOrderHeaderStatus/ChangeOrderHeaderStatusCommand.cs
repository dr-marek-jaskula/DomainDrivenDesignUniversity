using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

public sealed record ChangeOrderHeaderStatusCommand
(
    OrderHeaderId OrderHeaderId,
    ChangeOrderHeaderStatusCommand.ChangeOrderHeaderStatusRequestBody Body
) : ICommand<ChangeOrderHeaderStatusResponse>
{
    public sealed record ChangeOrderHeaderStatusRequestBody(OrderStatus NewOrderHeaderStatus);
}
