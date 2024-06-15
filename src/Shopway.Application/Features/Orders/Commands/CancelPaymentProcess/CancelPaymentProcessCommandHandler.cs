using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Application.Features.Orders.Commands.CancelPaymentProcess;

internal sealed class CancelPaymentProcessCommandHandler
(
    IOrderHeaderRepository orderHeaderRepository,
    IValidator validator,
    IPaymentGatewayService paymentGatewayService
)
    : ICommandHandler<CancelPaymentProcessCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;
    private readonly IValidator _validator = validator;
    private readonly IPaymentGatewayService _paymentGatewayService = paymentGatewayService;

    public async Task<IResult> Handle(CancelPaymentProcessCommand command, CancellationToken cancellationToken)
    {
        Result<(string SessionId, PaymentStatus _)> paymentProcessResult = await _paymentGatewayService.GetPaymentProcessResult();

        _validator
            .Validate(paymentProcessResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var sessionId = paymentProcessResult.Value.SessionId;

        var orderHeader = await _orderHeaderRepository.GetByPaymentSessionIdAsync(sessionId, cancellationToken);

        _validator
            .If(orderHeader is null, Error.NotFound<OrderHeader>(sessionId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        return orderHeader!.SetPaymentStatus(PaymentStatus.Canceled, sessionId);
    }
}
