using Shopway.Domain.DomainEvents;
using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Order : AggregateRoot
{
    public Amount Amount { get; private set; }
    public Status Status { get; private set; }
    //TODO HANDLE deadlines both for Order and Payment
    public DateTimeOffset Deadline { get; private set; }
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
        Discount discount,
        DateTimeOffset paymentDeadline)
        : base(id)
    {
        ProductId = productId;
        Amount = amount;
        CustomerId = customerId;
        Payment = CreateNewPayment(id, discount);
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

    internal decimal CalculateTotal()
    {
        return Math.Round(Product.Price.Value * Amount.Value * (1 - Payment.Discount.Value), 2);
    }

    private static Payment CreateNewPayment(Guid orderId, Discount discount, DateTimeOffset deadline)
    {
        return new Payment
        (
            id: Guid.NewGuid(),
            discount: discount,
            status: Status.New,
            deadline: deadline,
            orderId: orderId
        );
    }
}