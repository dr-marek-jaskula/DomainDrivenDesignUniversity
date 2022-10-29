using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;
//TODO make field private or internal!!?
public sealed class Payment : Entity
{
    public Discount Discount { get; private set; }
    public Status Status { get; private set; }
    public DateTimeOffset? OccurredOn { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }

    internal Payment(
        Guid id,
        Discount discount,
        Status status,
        Guid orderId)
        : base(id)
    {
        Discount = discount;
        Status = status;
        OccurredOn = null;
        OrderId = orderId;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public decimal Total => Order.CalculateTotal();
}