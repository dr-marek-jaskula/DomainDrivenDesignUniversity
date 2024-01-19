using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Enums;
using Shopway.Domain.Orders.Enumerations;

namespace Shopway.Application.Features.Orders.Queries;

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