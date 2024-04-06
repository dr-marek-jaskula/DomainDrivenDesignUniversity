using Shopway.Domain.Common.BaseTypes;

namespace Shopway.Domain.Orders.Events;

public sealed record NewPaymentAddedDomainEvent(Ulid Id, PaymentId OrderLineId, OrderHeaderId OrderHeaderId) : DomainEvent(Id)
{
    public static NewPaymentAddedDomainEvent New(PaymentId OrderLineId, OrderHeaderId OrderHeaderId)
    {
        return new NewPaymentAddedDomainEvent(Ulid.NewUlid(), OrderLineId, OrderHeaderId);
    }
}
