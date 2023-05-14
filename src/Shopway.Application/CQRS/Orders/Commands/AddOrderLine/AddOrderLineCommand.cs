using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Orders.Commands.AddOrderLine.AddOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.AddOrderLine;

public sealed record AddOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    ProductId ProductId,
    AddOrderLineRequestBody Body
) : ICommand<AddOrderLineResponse>
{
    public sealed record AddOrderLineRequestBody(int Amount, decimal? Discount);
}