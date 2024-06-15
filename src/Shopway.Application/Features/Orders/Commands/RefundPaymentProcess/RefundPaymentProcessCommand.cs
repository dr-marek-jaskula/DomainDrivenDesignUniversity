using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.RefundPaymentProcess;

public sealed record RefundPaymentProcessCommand(OrderHeaderId OrderHeaderId, PaymentId PaymentId) : ICommand;
