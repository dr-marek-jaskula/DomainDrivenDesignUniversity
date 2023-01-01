using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand
(
    Guid ProductId,
    int Amount,
    Guid CustomerId,
    decimal? Discount
) : ICommand<CreateOrderResponse>;

