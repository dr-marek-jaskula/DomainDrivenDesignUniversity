using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;
using static Shopway.Application.Features.Orders.Commands.UpdateOrderLine.UpdateOrderLineCommand;

namespace Shopway.Application.Features.Orders.Commands.UpdateOrderLine;

public sealed record UpdateOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    OrderLineId OrderLineId,
    UpdateOrderLineRequestBody Body
) : ICommand<UpdateOrderLineResponse>
{
    public sealed record UpdateOrderLineRequestBody(int Amount, decimal? Discount);
}
