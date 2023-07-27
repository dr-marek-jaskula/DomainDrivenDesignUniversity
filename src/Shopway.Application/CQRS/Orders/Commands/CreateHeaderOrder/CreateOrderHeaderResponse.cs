using Shopway.Application.Abstractions;
namespace Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;

public sealed record CreateOrderHeaderResponse
(
    Ulid Id
) : IResponse;