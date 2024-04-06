using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Domain.Orders;

public interface IPaymentGatewayService
{
    Task<Result<(string SessionId, PaymentStatus PaymentStatus)>> GetPaymentProcessResult();
    Task<Result> Refund(Session session);
    Task<Result<Domain.Orders.ValueObjects.Session>> StartSessionAsync(OrderHeader orderHeader);
}