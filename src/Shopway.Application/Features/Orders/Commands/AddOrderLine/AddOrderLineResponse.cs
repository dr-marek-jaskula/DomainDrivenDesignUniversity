using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Commands.AddOrderLine;

public sealed record AddOrderLineResponse
(
    Ulid Id
) : IResponse;
