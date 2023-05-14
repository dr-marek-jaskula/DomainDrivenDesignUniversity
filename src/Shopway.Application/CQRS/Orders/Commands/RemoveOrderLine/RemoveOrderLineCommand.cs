using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.CQRS.Orders.Commands.RemoveOrderLine;

public sealed record RemoveOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    OrderLineId OrderLineId
) : ICommand<RemoveOrderLineResponse>;