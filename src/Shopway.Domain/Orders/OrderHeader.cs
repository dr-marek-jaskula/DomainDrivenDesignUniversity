using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Domain.Orders.Events;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Domain.Users;
using static Shopway.Domain.Orders.Enumerations.OrderStatus;
using static Shopway.Domain.Orders.Enumerations.PaymentStatus;
using static Shopway.Domain.Orders.Errors.DomainErrors.Status;

namespace Shopway.Domain.Orders;

[GenerateEntityId]
public sealed class OrderHeader : AggregateRoot<OrderHeaderId>, IAuditable, ISoftDeletable
{
    private readonly List<OrderLine> _orderLines = [];
    private readonly List<Payment> _payments = [];
    private bool PaymentReceived => _payments.Any(payment => payment.Status is Received);

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
        _payments.Add(Payment.Create());
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
    public UserId UserId { get; private set; }
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    public DateTimeOffset? SoftDeletedOn { get; set; }
    public bool SoftDeleted { get; set; }

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

    public Result AddOrderLine(OrderLine orderLine)
    {
        if (Status is not New)
        {
            return Result.Failure(Errors.DomainErrors.AddOrderLineError.InvalidOrderHeaderStatus);
        }

        _orderLines.Add(orderLine);
        RaiseDomainEvent(OrderLineAddedDomainEvent.New(orderLine.Id, Id));
        return Result.Success();
    }

    public Payment AddPayment(Payment payment)
    {
        _payments.Add(payment);
        RaiseDomainEvent(NewPaymentAddedDomainEvent.New(payment.Id, Id));
        return payment;
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

    public decimal CalculateTotalCost()
    {
        decimal totalCost = 0;

        foreach (var orderLine in OrderLines)
        {
            totalCost += orderLine.CalculateLineCost();
        }

        return Math.Round(totalCost * (1 - TotalDiscount.Value), 2);
    }

    public void SoftDelete()
    {
        SoftDeleted = true;
        SoftDeletedOn = DateTimeOffset.UtcNow;
    }

    public Result SetPaymentStatus(PaymentStatus paymentStatus, string sessionId)
    {
        var payment = Payments
            .FirstOrDefault(x => x.Session!.Id == sessionId);

        if (payment is null)
        {
            return Result.Failure(Error.NotFound(nameof(Payment), $"SessionId: '{sessionId}'", "To set Payment Status the valid sessionId should be provided"));
        }

        payment.SetStatus(paymentStatus);

        if (Status is New && paymentStatus.IsReceivedOrConfirmed())
        {
            var changeStatusResult = ChangeStatus(InProgress);

            if (changeStatusResult.IsFailure)
            {
                return changeStatusResult;
            }
        }

        return Result.Success();
    }

    public async Task<Result> Refund(PaymentId paymentId, IPaymentGatewayService paymentGatewayService)
    {
        var paymentToRefund = Payments
            .Where(p => p.Id == paymentId)
            .FirstOrDefault();

        if (paymentToRefund is null)
        {
            return Result.Failure(Error.NotFound<Payment>(paymentId));
        }

        var refundResult = await paymentToRefund!.Refund(paymentGatewayService);

        if (refundResult.IsFailure)
        {
            return refundResult;
        }

        if (Status.NotSent())
        {
            ChangeStatus(Rejected);
        }

        return Result.Success();
    }
}
