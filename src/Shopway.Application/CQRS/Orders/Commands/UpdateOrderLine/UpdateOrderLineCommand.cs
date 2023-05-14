using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine.UpdateOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine;

public sealed record UpdateOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    OrderLineId OrderLineId,
    UpdateOrderLineRequestBody Body
) : ICommand<UpdateOrderLineResponse>
{
    public sealed record UpdateOrderLineRequestBody(int Amount, decimal? Discount);
}
