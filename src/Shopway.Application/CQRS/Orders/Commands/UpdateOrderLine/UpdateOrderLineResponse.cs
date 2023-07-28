using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.UpdateOrderLine;

public sealed record UpdateOrderLineResponse
(
    Ulid Id
) : IResponse;