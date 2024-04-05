using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;
using static Shopway.Domain.Orders.Enumerations.OrderStatus;

namespace Shopway.Application.Features.Orders.Commands.FinalizePaymentProcess;

internal sealed class FinalizePaymentProcessCommandHandler
(
    IOrderHeaderRepository orderHeaderRepository, 
    IValidator validator, 
    IPaymentGatewayService paymentGatewayService
)
    : ICommandHandler<FinalizePaymentProcessCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;
    private readonly IValidator _validator = validator;
    private readonly IPaymentGatewayService _paymentGatewayService = paymentGatewayService;

    public async Task<IResult> Handle(FinalizePaymentProcessCommand command, CancellationToken cancellationToken)
    {
        var paymentProcessResult = await _paymentGatewayService.GetPaymentProcessResult();

        _validator
            .Validate(paymentProcessResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        (string sessionId, PaymentStatus paymentStatus) = paymentProcessResult.Value;

        var orderHeader = await _orderHeaderRepository.GetByPaymentSessionIdAsync(sessionId, cancellationToken);

        _validator
            .If(orderHeader is null, Error.NotFound<OrderHeader>(sessionId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        orderHeader!.Payments
            .Single(x => x.Session!.Id == sessionId)
            .SetStatus(paymentStatus);

        if (orderHeader.Status is New && paymentStatus.IsReceivedOrConfirmed())
        {
            orderHeader.ChangeStatus(InProgress);
        }

        return Result.Success();
    }
}