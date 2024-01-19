using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;

public sealed record SoftDeleteOrderHeaderCommand
(
    OrderHeaderId OrderHeaderId
) : ICommand;

