using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;

public sealed record CreateOrderHeaderCommand
(
    Guid UserId,
    decimal? Discount
) : ICommand<CreateOrderHeaderResponse>;

