using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Entities;

public sealed class Payment : Entity<PaymentId>, IAuditable
{
    private Payment
    (
        PaymentId id,
        Discount totalDiscount,
        PaymentStatus status,
        OrderHeader orderHeader
    )
        : base(id)
    {
        TotalDiscount = totalDiscount;
        Status = status;
        OrderHeader = orderHeader;
        OrderHeaderId = orderHeader.Id;
    }

    // Empty constructor in this case is required by EF Core
    private Payment()
    {
    }

    public Discount TotalDiscount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public OrderHeader OrderHeader { get; private set; }
    public OrderHeaderId OrderHeaderId { get; private set; }

    public static Payment Create(OrderHeader orderHeader, Discount totalDiscount)
    {
        return new Payment
        (
            id: PaymentId.New(),
            totalDiscount: totalDiscount,
            status: PaymentStatus.NotReceived,
            orderHeader: orderHeader
        );
    }

    public decimal CalculateTotalPayment()
    {
        decimal totalPayment = 0;

        foreach (var orderLine in OrderHeader.OrderLines)
        {
            totalPayment += orderLine.CalculateLineCost();
        }

        return Math.Round(totalPayment * (1 - TotalDiscount.Value), 2);
    }
}