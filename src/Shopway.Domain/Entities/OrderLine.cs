using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class OrderLine : Entity<OrderLineId>, IAuditable
{
    private OrderLine
    (
        OrderLineId id,
        ProductId productId,
        OrderHeaderId orderHeaderId,
        Amount amount,
        Discount lineDiscount
    )
        : base(id)
    {
        ProductId = productId;
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
    public Product Product { get; private set; }
    public ProductId ProductId { get; private set; }
    public OrderHeaderId OrderHeaderId { get; private set; }

    public static OrderLine Create
    (
        OrderLineId id,
        ProductId productId,
        OrderHeaderId orderHeaderId,
        Amount amount,
        Discount lineDiscount
    )
    {
        var order = new OrderLine
        (
            id,
            productId,
            orderHeaderId,
            amount,
            lineDiscount
        );

        return order;
    }

    public decimal CalculateLineCost()
    {
        return Math.Round(Product.Price.Value * Amount.Value * (1 - LineDiscount.Value), 2);
    }
}