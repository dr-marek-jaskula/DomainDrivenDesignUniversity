using Shopway.Application.Features.Orders.Queries;
using Shopway.Domain.Orders;

namespace Shopway.Application.Mappings;

public static class PaymentMapping
{
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
