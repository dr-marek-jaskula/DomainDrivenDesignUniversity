using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Features.Orders.Commands.StartPaymentProcess;

internal sealed class StartPaymentProcessCommandHandler(IOrderHeaderRepository orderHeaderRepository)
    : ICommandHandler<StartPaymentProcessCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    public async Task<IResult> Handle(StartPaymentProcessCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithIncludesAsync(command.OrderHeaderId, cancellationToken, oh => oh.Payment);

        var sessionIdResult = SessionId.Create(command.Body.SessionId);

        if (sessionIdResult.IsFailure)
        {
            return sessionIdResult;
        }
        
        orderHeader.Payment.SetSessionId(sessionIdResult.Value);

        return Result.Success();
    }
}