using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Commands.ChangeOrderHeaderStatus;

public sealed record ChangeOrderHeaderStatusResponse
(
    Ulid Id
) : IResponse;