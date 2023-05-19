using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.CQRS.Orders.Commands.CreateHeaderOrder;

public sealed record CreateOrderHeaderCommand
(
    UserId UserId,
    decimal? Discount
) : ICommand<CreateOrderHeaderResponse>;

