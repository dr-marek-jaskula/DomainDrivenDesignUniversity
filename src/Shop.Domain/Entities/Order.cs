using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Order : AggregateRoot
{
    public int Amount { get; set; }
    public Status Status { get; set; }
    public DateTime Deadline { get; set; }
    public virtual Product? Product { get; set; }
    public int? ProductId { get; set; }
    public virtual Payment? Payment { get; set; }
    public int? PaymentId { get; set; }
    public virtual Shop? Shop { get; set; }
    public int? ShopId { get; set; }
    public virtual Customer? Customer { get; set; }
    public int? CustomerId { get; set; }
}