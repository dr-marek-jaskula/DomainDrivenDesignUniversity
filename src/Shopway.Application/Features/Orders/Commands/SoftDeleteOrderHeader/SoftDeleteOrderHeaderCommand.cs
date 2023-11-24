using Shopway.Domain.Orders;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;

public sealed record SoftDeleteOrderHeaderCommand
(
    OrderHeaderId OrderHeaderId
) : ICommand;

