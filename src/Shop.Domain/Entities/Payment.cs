using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Payment : Entity
{
    public Discount Discount { get; private set; }
    public Status Status { get; private set; }
    public DateTimeOffset Deadline { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }

    internal Payment(
        Guid id,
        Discount discount,
        Status status,
        DateTimeOffset deadline,
        Guid orderId)
        : base(id)
    {
        Discount = discount;
        Status = status;
        Deadline = deadline;
        OrderId = orderId;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public decimal Total => Order.CalculateTotal();
}