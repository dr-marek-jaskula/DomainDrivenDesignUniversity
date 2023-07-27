using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.ChangeOrderHeaderStatus;

public sealed record ChangeOrderHeaderStatusResponse
(
    Ulid Id
) : IResponse;