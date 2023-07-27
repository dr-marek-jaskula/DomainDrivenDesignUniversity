using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.RemoveOrderLine;

public sealed record RemoveOrderLineResponse
(
    Ulid Id
) : IResponse;