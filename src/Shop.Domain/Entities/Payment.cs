using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Payment : Entity
{
    public Payment(Guid id, decimal? discount, decimal total, Status status, DateTime deadline) : base(id)
    {
        Discount = discount;
        Total = total;
        Status = status;
        Deadline = deadline;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public decimal? Discount { get; private set; }
    public decimal Total { get; private set; }
    public Status Status { get; private set; }
    public DateTime Deadline { get; private set; }
    public Order? Order { get; private set; }
}