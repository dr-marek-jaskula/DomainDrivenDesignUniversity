using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.RemoveOrderLine;

public sealed record RemoveOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    OrderLineId OrderLineId
) : ICommand<RemoveOrderLineResponse>;