using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Application.Mappings;

namespace Shopway.Application.Features.Orders.Commands.StartPaymentProcess;

internal sealed class StartPaymentProcessCommandHandler(IOrderHeaderRepository orderHeaderRepository, IPaymentGatewayService paymentGatewayService)
    : ICommandHandler<StartPaymentProcessCommand, StartPaymentProcessResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;
    private readonly IPaymentGatewayService _paymentGatewayService = paymentGatewayService;

    public async Task<IResult<StartPaymentProcessResponse>> Handle(StartPaymentProcessCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithIncludesAsync(command.OrderHeaderId, cancellationToken, oh => oh.Payments, oh => oh.OrderLines);
        var sessionResult = await _paymentGatewayService.StartSessionAsync(orderHeader);

        if (sessionResult.IsFailure)
        {
            return sessionResult.Error
                .ToResult<StartPaymentProcessResponse>();
        }

        var payment = orderHeader.Payments.FirstOrDefault(x => x.Session is null)
            ?? orderHeader.AddPayment(Payment.Create());

        payment.SetSession(sessionResult.Value);

        return sessionResult.Value
            .ToResponse()
            .ToResult();
    }
}
