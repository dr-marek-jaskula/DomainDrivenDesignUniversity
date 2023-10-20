using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Commands.RemoveOrderLine;

public sealed record RemoveOrderLineResponse
(
    Ulid Id
) : IResponse;