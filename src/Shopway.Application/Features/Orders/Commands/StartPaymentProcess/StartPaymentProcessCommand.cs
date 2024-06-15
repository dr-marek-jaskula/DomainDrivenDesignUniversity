using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Commands.StartPaymentProcess;

public sealed record StartPaymentProcessCommand(OrderHeaderId OrderHeaderId) : ICommand<StartPaymentProcessResponse>;
