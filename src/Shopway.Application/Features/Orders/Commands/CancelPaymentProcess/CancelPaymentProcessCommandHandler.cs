using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
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

        var orderHeader = await _orderHeaderRepository.GetByPaymentSessionIdAsync(paymentProcessResult.Value.SessionId, cancellationToken);

        _validator
            .If(orderHeader is null, Error.NotFound<OrderHeader>(paymentProcessResult.Value.SessionId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        orderHeader!.Payments
            .Single(x => x.Session!.Id == paymentProcessResult.Value.SessionId)
            .SetStatus(PaymentStatus.Canceled);

        return Result.Success();
    }
}