using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Entities;

public sealed class OrderLine : Entity<OrderLineId>, IAuditable
{
    private OrderLine
    (
        OrderLineId id,
        ProductSummary productSummary,
        OrderHeaderId orderHeaderId,
        Amount amount,
        Discount lineDiscount
    )
        : base(id)
    {
        ProductSummary = productSummary;
        OrderHeaderId = orderHeaderId;
        Amount = amount;
        LineDiscount = lineDiscount;
    }

    // Empty constructor in this case is required by EF Core
    private OrderLine()
    {
    }

    public Amount Amount { get; private set; }
    public Discount LineDiscount { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public ProductSummary ProductSummary { get; private set; }
    public OrderHeaderId OrderHeaderId { get; private set; }

    public static OrderLine Create
    (
        OrderLineId id,
        ProductSummary productSummary,
        OrderHeaderId orderHeaderId,
        Amount amount,
        Discount lineDiscount
    )
    {
        return new OrderLine
        (
            id,
            productSummary,
            orderHeaderId,
            amount,
            lineDiscount
        );
    }

    public decimal CalculateLineCost()
    {
        return Math.Round(ProductSummary.Price.Value * Amount.Value * (1 - LineDiscount.Value), 2);
    }

    public void UpdateAmount(Amount amount)
    {
        Amount = amount;
    }

    public void UpdateDiscount(Discount discount)
    {
        LineDiscount = discount;
    }
}