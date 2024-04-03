using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Orders;

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

        var orderHeader = await _orderHeaderRepository.GetByPaymentSessionIdWithIncludesAsync(paymentProcessResult.Value.SessionId, cancellationToken, oh => oh.Payment);

        _validator
            .If(orderHeader is null, Error.NotFound<OrderHeader>(paymentProcessResult.Value.SessionId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        orderHeader!.Payment.SetStatus(paymentProcessResult.Value.PaymentStatus);

        return Result.Success();
    }
}