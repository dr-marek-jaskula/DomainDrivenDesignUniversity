using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.StartPaymentProcess;

internal sealed class StartPaymentProcessCommandHandler(IOrderHeaderRepository orderHeaderRepository, IPaymentGatewayService paymentGatewayService)
    : ICommandHandler<StartPaymentProcessCommand, StartPaymentProcessResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;
    private readonly IPaymentGatewayService _paymentGatewayService = paymentGatewayService;

    public async Task<IResult<StartPaymentProcessResponse>> Handle(StartPaymentProcessCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithIncludesAsync(command.OrderHeaderId, cancellationToken, oh => oh.Payment, oh => oh.OrderLines);
        var sessionResult = await _paymentGatewayService.StartSessionAsync(orderHeader);

        if (sessionResult.IsFailure)
        {
            return Result.Failure<StartPaymentProcessResponse>(sessionResult.Error);
        }

        orderHeader.Payment.SetSession(sessionResult.Value);

        return new StartPaymentProcessResponse(sessionResult.Value.Id, sessionResult.Value.Secret)
            .ToResult();
    }
}