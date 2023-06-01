using Shopway.Domain.Results;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors.Status;
using static Shopway.Domain.Enums.OrderStatus;
using static Shopway.Domain.Enums.PaymentStatus;

namespace Shopway.Domain.Entities;

public sealed class OrderHeader : AggregateRoot<OrderHeaderId>, IAuditable
{
    private readonly List<OrderLine> _orderLines = new();
    private bool PaymentReceived => Payment.Status is Received;

    private OrderHeader
    (
        OrderHeaderId id,
        UserId userId,
        Discount discount
    )
        : base(id)
    {
        UserId = userId;
        TotalDiscount = discount;
        Payment = Payment.Create(this);
        Status = New;
    }

    // Empty constructor in this case is required by EF Core
    private OrderHeader()
    {
    }

    public Discount TotalDiscount { get; private set; }
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

    public bool RemoveOrderLine(OrderLine orderLine)
    {
        return _orderLines.Remove(orderLine);
    }

    public Result ChangeStatus(OrderStatus newOrderStatus)
    {
        if (Status.CanBeChangedTo(newOrderStatus) is false)
        {
            return Result.Failure(InvalidStatusChange(Status, newOrderStatus));
        }

        if (newOrderStatus is InProgress && PaymentReceived is false)
        {
            return Result.Failure(PaymentNotReceived);
        }

        Status = newOrderStatus;
        return Result.Success();
    }

    public decimal CalculateTotalPrice()
    {
        decimal totalPayment = 0;

        foreach (var orderLine in OrderLines)
        {
            totalPayment += orderLine.CalculateLineCost();
        }

        return Math.Round(totalPayment * (1 - TotalDiscount.Value), 2);
    }
}