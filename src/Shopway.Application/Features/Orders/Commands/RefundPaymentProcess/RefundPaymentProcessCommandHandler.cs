using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.RefundPaymentProcess;

internal sealed class RefundPaymentProcessCommandHandler
(
    IOrderHeaderRepository orderHeaderRepository,
    IValidator validator,
    IPaymentGatewayService paymentGatewayService
)
    : ICommandHandler<RefundPaymentProcessCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;
    private readonly IValidator _validator = validator;
    private readonly IPaymentGatewayService _paymentGatewayService = paymentGatewayService;

    public async Task<IResult> Handle(RefundPaymentProcessCommand command, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository.GetByIdWithIncludesAsync(command.OrderHeaderId, cancellationToken, oh => oh.Payments);

        _validator
            .If(orderHeader is null, Error.NotFound<OrderHeader>(command.OrderHeaderId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var paymentToRefund = orderHeader!.Payments
            .Where(p => p.Id == command.PaymentId)
            .FirstOrDefault();

        _validator
            .If(paymentToRefund is null, Error.NotFound<Payment>(command.PaymentId));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        return await paymentToRefund!.Refund(_paymentGatewayService);
    }
}