using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Application.Abstractions;

public interface IPaymentGatewayService
{
    Task<Result<(string SessionId, PaymentStatus PaymentStatus)>> GetPaymentProcessResult();
    Task<Result<Domain.Orders.ValueObjects.Session>> StartSessionAsync(OrderHeader orderHeader);
}