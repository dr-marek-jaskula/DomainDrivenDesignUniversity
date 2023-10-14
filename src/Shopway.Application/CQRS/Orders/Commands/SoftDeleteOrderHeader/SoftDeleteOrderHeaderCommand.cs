using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Orders.Commands.SoftDeleteOrderHeader;

public sealed record SoftDeleteOrderHeaderCommand
(
    OrderHeaderId OrderHeaderId
) : ICommand;

