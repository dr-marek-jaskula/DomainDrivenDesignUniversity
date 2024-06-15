using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Commands.UpdateOrderLine;

public sealed record UpdateOrderLineResponse
(
    Ulid Id
) : IResponse;
