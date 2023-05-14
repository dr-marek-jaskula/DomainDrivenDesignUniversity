using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.RemoveOrderLine;

public sealed record RemoveOrderLineResponse
(
    Guid Id
) : IResponse;