using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class OrderHeader : AggregateRoot<OrderHeaderId>, IAuditable
{
    private readonly List<OrderLine> _orderLines = new();

    private OrderHeader
    (
        OrderHeaderId id,
        UserId userId,
        Discount discount
    )
        : base(id)
    {
        UserId = userId;
        Payment = Payment.Create(this, discount);
        Status = OrderStatus.New;
    }

    // Empty constructor in this case is required by EF Core
    private OrderHeader()
    {
    }

    public OrderStatus Status { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public Payment Payment { get; private set; }
    public PaymentId PaymentId { get; private set; }
    public User User { get; private set; }
    public UserId UserId { get; private set; }
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

    public static OrderHeader Create
    (
        OrderHeaderId id,
        UserId userId,
        Discount discount
    )
    {
        var orderHeader = new OrderHeader
        (
            id,
            userId,
            discount
        );

        orderHeader.RaiseDomainEvent(OrderHeaderCreatedDomainEvent.New(orderHeader.Id));

        return orderHeader;
    }

    public OrderLine AddOrderLine(OrderLine orderLine)
    {
        _orderLines.Add(orderLine);

        RaiseDomainEvent(OrderLineAddedDomainEvent.New(orderLine.Id, Id));

        return orderLine;
    }
}