using Shopway.Application.Features.Orders.Commands.StartPaymentProcess;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Mappings;

public static class PaymentMapping
{
    public static StartPaymentProcessResponse ToResponse(this Session session)
    {
        return new StartPaymentProcessResponse(session.Id, session.Secret);
    }

    public static PaymentResponse ToResponse(this Payment payment)
    {
        return new PaymentResponse
        (
            payment.Id.Value,
            payment.Status.ToString(),
            payment.IsRefunded.ToString()
        );
    }

    public static IReadOnlyCollection<PaymentResponse> ToResponses(this IReadOnlyCollection<Payment> payments)
    {
        return payments
            .Select(ToResponse)
            .ToList()
            .AsReadOnly();
    }
}
