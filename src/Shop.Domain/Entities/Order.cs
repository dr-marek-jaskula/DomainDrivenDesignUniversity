using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Order : AggregateRoot
{

    // Empty constructor in this case is required by EF Core
    private Order()
    {
    }

    public int Amount { get; private set; }
    public Status Status { get; private set; }
    public DateTime Deadline { get; private set; }
    public Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public Payment? Payment { get; private set; }
    public int? PaymentId { get; private set; }
    public Shop? Shop { get; private set; }
    public int? ShopId { get; private set; }
    public Customer? Customer { get; private set; }
    public int? CustomerId { get; private set; }
}