using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
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

        return await orderHeader!.Refund(command.PaymentId, _paymentGatewayService);
    }
}
