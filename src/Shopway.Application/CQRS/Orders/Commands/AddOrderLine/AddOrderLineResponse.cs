using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.AddOrderLine;

public sealed record AddOrderLineResponse
(
    Ulid Id
) : IResponse;