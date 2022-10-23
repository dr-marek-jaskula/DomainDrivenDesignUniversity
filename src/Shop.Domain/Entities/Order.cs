using Shopway.Domain.DomainEvents;
using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Order : AggregateRoot
{
    public Amount Amount { get; private set; }
    public Status Status { get; private set; }
    public DateTime Deadline { get; private set; }
    public Product Product { get; private set; }
    public int ProductId { get; private set; }
    public Payment Payment { get; private set; }
    public int PaymentId { get; private set; }
    public Customer Customer { get; private set; }
    public int CustomerId { get; private set; }

    public Order(
        Guid id,
        Product product,
        Amount amount,
        Customer customer,
        Payment payment,
        Status status)
        : base(id)
    {
        Product = product;
        Amount = amount;
        Customer = customer;
        Payment = payment;
        Status = status;
    }

    // Empty constructor in this case is required by EF Core
    private Order()
    {
    }

    public static Order Create(
        Guid id,
        Product product,
        Amount amount,
        Customer customer,
        Payment payment,
        Status status)
    {
        var order = new Order(
            id,
            product,
            amount,
            customer,
            payment,
            status);

        order.RaiseDomainEvent(new OrderRegisteredDomainEvent(Guid.NewGuid(), order.Id));

        return order;
    }

    internal decimal CalculateTotal()
    {
        return Math.Round(Product.Price.Value * Amount.Value * (1 - Payment.Discount.Value), 2);
    }
}