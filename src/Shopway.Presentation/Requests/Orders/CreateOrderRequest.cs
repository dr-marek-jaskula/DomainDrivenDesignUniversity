namespace Shopway.Presentation.Requests.Orders;

public sealed record CreateOrderRequest
(
    Guid ProductId,
    int Amount,
    Guid CustomerId,
    decimal? Discount = null
);