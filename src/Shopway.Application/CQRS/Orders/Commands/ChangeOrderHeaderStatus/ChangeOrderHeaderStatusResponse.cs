using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders.Commands.ChangeOrderHeaderStatus;

public sealed record ChangeOrderHeaderStatusResponse
(
    Guid Id
) : IResponse;