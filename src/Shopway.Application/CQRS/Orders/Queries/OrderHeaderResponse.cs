using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions.Common;
using Shopway.Domain.Enums;

namespace Shopway.Application.CQRS.Orders.Queries;

public sealed record OrderHeaderResponse
(
    Ulid Id,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    decimal TotalPayment,
    decimal TotalDiscount,
    IReadOnlyCollection<OrderLineResponse> OrderLines
) 
    : IResponse, IHasCursor;