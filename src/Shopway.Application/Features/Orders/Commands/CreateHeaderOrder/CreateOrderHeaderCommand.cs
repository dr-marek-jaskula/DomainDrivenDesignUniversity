using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

public sealed record CreateOrderHeaderCommand
(
    UserId UserId,
    decimal? Discount
) : ICommand<CreateOrderHeaderResponse>;

