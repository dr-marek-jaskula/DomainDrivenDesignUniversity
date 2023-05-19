using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Enums;

namespace Shopway.Application.CQRS.Orders.Commands.ChangeOrderHeaderStatus;

public sealed record ChangeOrderHeaderStatusCommand
(
    OrderHeaderId OrderHeaderId,
    ChangeOrderHeaderStatusCommand.ChangeOrderHeaderStatusRequestBody Body
) : ICommand<ChangeOrderHeaderStatusResponse>
{
    public sealed record ChangeOrderHeaderStatusRequestBody(OrderStatus NewOrderHeaderStatus);
}
