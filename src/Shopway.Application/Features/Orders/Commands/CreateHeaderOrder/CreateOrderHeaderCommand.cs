using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

public sealed record CreateOrderHeaderCommand
(
    UserId UserId,
    decimal? Discount
) : ICommand<CreateOrderHeaderResponse>;

