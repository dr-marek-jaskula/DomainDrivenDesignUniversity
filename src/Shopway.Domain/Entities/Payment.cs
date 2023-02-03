using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Entities;

public sealed class Payment : Entity<PaymentId>, IAuditableEntity
{
    internal Payment
    (
        PaymentId id,
        Discount discount,
        Status status,
        OrderId orderId
    )
        : base(id)
    {
        Discount = discount;
        Status = status;
        OrderId = orderId;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public Discount Discount { get; private set; }
    public Status Status { get; private set; }
    public OrderId OrderId { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    internal static Payment Create(OrderId orderId, Discount discount)
    {
        return new Payment
        (
            id: PaymentId.New(),
            discount: discount,
            status: Status.New,
            orderId: orderId
        );
    }
}