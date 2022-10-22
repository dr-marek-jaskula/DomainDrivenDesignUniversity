using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Order : AggregateRoot
{
    public int Amount { get; private set; }
    public Status Status { get; private set; }
    public DateTime Deadline { get; private set; }
    public virtual Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public virtual Payment? Payment { get; private set; }
    public int? PaymentId { get; private set; }
    public virtual Shop? Shop { get; private set; }
    public int? ShopId { get; private set; }
    public virtual Customer? Customer { get; private set; }
    public int? CustomerId { get; private set; }
}