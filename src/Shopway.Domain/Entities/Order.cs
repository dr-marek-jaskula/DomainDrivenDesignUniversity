using Shopway.Domain.DomainEvents;
using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Order : AggregateRoot<OrderId>, IAuditableEntity
{
    public Amount Amount { get; private set; }
    public Status Status { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public Product Product { get; private set; }
    public Guid ProductId { get; private set; }
    public Payment Payment { get; private set; }
    public Guid PaymentId { get; private set; }
    public Customer Customer { get; private set; }
    public Guid CustomerId { get; private set; }

    internal Order(
        Guid id,
        Guid productId,
        Amount amount,
        Guid customerId,
        Discount discount)
        : base(id)
    {
        ProductId = productId;
        Amount = amount;
        CustomerId = customerId;
        Payment = Payment.Create(id, discount);
        Status = Status.New;
    }

    // Empty constructor in this case is required by EF Core
    private Order()
    {
    }

    public static Order Create(
        Guid id,
        Guid productId,
        Amount amount,
        Guid customerId,
        Discount discount)
    {
        var order = new Order(
            id,
            productId,
            amount,
            customerId,
            discount);

        order.RaiseDomainEvent(new OrderCreatedDomainEvent(Guid.NewGuid(), order.Id));

        return order;
    }

    public decimal CalculateTotalPayment()
    {
        return Math.Round(Product.Price.Value * Amount.Value * (1 - Payment.Discount.Value), 2);
    }
}