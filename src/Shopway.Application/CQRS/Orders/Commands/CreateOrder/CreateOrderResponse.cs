using Shopway.Application.Abstractions;
namespace Shopway.Application.CQRS.Orders.Commands.CreateOrder;

public sealed record CreateOrderResponse
(
    Guid Id
) : IResponse;