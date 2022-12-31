using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.UpdateOrder;

public sealed record UpdateOrderResponse
(
    Guid Id
) : IResponse;