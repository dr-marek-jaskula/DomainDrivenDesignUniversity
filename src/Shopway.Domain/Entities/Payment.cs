using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Payment : Entity
{
    public Discount Discount { get; private set; }
    public Status Status { get; private set; }
    public DateTimeOffset? OccurredOn { get; private set; }
    public Guid OrderId { get; private set; }

    internal Payment
    (
        Guid id,
        Discount discount,
        Status status,
        Guid orderId
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

    internal static Payment Create(Guid orderId, Discount discount)
    {
        return new Payment
        (
            id: Guid.NewGuid(),
            discount: discount,
            status: Status.New,
            orderId: orderId
        );
    }
}