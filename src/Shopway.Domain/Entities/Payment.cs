using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;
using static Shopway.Domain.Enums.PaymentStatus;

namespace Shopway.Domain.Entities;

public sealed class Payment : AggregateRoot<PaymentId>, IAuditable
{
    private Payment
    (
        PaymentId id,
        PaymentStatus status,
        OrderHeader orderHeader
    )
        : base(id)
    {
        Status = status;
        OrderHeader = orderHeader;
        OrderHeaderId = orderHeader.Id;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public PaymentStatus Status { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public OrderHeader OrderHeader { get; private set; }
    public OrderHeaderId OrderHeaderId { get; private set; }

    public static Payment Create(OrderHeader orderHeader)
    {
        return new Payment
        (
            id: PaymentId.New(),
            status: NotReceived,
            orderHeader: orderHeader
        );
    }

    public void PaymentReceived()
    {
        Status = Received;
    }
}