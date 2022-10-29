using Shopway.Application.Abstractions.CQRS;
namespace Shopway.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand
(
    Guid ProductId,
    int Amount,
    Guid CustomerId,
    decimal? Discount
) : ICommand<Guid>;

