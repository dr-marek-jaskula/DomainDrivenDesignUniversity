using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

public sealed record ChangeOrderHeaderStatusCommand
(
    OrderHeaderId OrderHeaderId,
    ChangeOrderHeaderStatusCommand.ChangeOrderHeaderStatusRequestBody Body
) : ICommand<ChangeOrderHeaderStatusResponse>
{
    public sealed record ChangeOrderHeaderStatusRequestBody(OrderStatus NewOrderHeaderStatus);
}
