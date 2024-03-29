using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;
using static Shopway.Application.Features.Orders.Commands.StartPaymentProcess.StartPaymentProcessCommand;

namespace Shopway.Application.Features.Orders.Commands.StartPaymentProcess;

public sealed record StartPaymentProcessCommand
(
    OrderHeaderId OrderHeaderId,
    StartPaymentProcessCommandBody Body
) : ICommand
{
    public sealed record StartPaymentProcessCommandBody(string SessionId);
}